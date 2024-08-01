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

namespace PersonalApp.Controllers
{
    public class MonthExpenseSheetController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        // GET: MonthExpenseSheet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MonthExpenseSheet()
        {
            ViewBag.head = "History";

            MonthExpense monthExpense = new MonthExpense();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM DayList";
                SqlCommand cmd = new SqlCommand(query, conn);
                DataSet ds = new DataSet();
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                List<MonthExpenseClass> images = new List<MonthExpenseClass>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MonthExpenseClass monthExpenseClass = new MonthExpenseClass();


                    monthExpenseClass.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                    monthExpenseClass.ImageUrl = ds.Tables[0].Rows[i]["Category"].ToString();
                    monthExpenseClass.IamgeName = ds.Tables[0].Rows[i]["ImageName"].ToString();
                    monthExpenseClass.Description = ds.Tables[0].Rows[i]["Descriptions"].ToString();
                    monthExpenseClass.Amount = ds.Tables[0].Rows[i]["Amount"].ToString();
                    monthExpenseClass.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dates"]);
                    images.Add(monthExpenseClass);
                }
                monthExpense.monthExpenseList = images;
            }

            return View(monthExpense);
        }


    }
}