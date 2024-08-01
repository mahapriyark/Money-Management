using PersonalApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PersonalApp.Models.MonthExpense;
using static PersonalApp.Models.Months;

namespace PersonalApp.Controllers
{
    public class MonthsController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        // GET: Months
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewMonth()
        {
            
            ViewBag.head = "Add Salary";
            return View();
        }

        [HttpPost]
        public ActionResult AddNewMonth(MonthsClass months)
        {
            
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Salary (UserName, SalaryDate, SalaryAmount, CompanyName) Values(@UserName, @SalaryDate, @SalaryAmount, @CompanyName)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserName", months.UserName);
                cmd.Parameters.AddWithValue("@SalaryDate", months.SalaryDate);
                cmd.Parameters.AddWithValue("@SalaryAmount", months.SalaryAmt);
                cmd.Parameters.AddWithValue("@CompanyName", months.CompanyName);

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("HomePage", "HomePage");
        }

        public ActionResult EditMonth()
        {
            return View();
        }

        public ActionResult DeleteMonth()
        {
            return View();
        }

        public ActionResult ViewAllMonths(Months months)
        {
            ViewBag.head = "View Salary";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Salary";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                List<MonthsClass> monthsList = new List<MonthsClass>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MonthsClass month = new MonthsClass();

                    month.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    month.SalaryAmt = Convert.ToInt32(ds.Tables[0].Rows[i]["SalaryAmount"]);
                    month.SalaryDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["SalaryDate"]);
                    month.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();

                    months.monthsClassesList.Add(month);
                }

            }

                return View(months);
        }
    }
}