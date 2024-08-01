using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PersonalApp.Models;

namespace PersonalApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public ActionResult Login(Login loginAc)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*), User_Priority FROM Users WHERE UserName = @UserName AND Password = @Password GROUP BY User_Priority";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserName", loginAc.UserName);
                    cmd.Parameters.AddWithValue("@Password", loginAc.Password);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        int result = rdr.GetInt32(0); // Assuming COUNT(*) returns an integer
                        if (result > 0)
                        {
                            loginAc.User_Priority = rdr.GetInt32(1); // Assuming User_Priority is an integer
                            FormsAuthentication.SetAuthCookie(loginAc.UserName, false);
                            Session["username"] = loginAc.UserName.ToString();
                            Session["priority"] = loginAc.User_Priority.ToString();
                            return RedirectToAction("HomePage", "HomePage");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid username or password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                    con.Close();
                }
            }
            // Clear the password field
            loginAc.Password = string.Empty;
            return View(loginAc);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return View("Login");
        }

      

    }
}