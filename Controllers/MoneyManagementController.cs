using PersonalApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PersonalApp.Models.MoneyManagement;
using static PersonalApp.Models.Todos;

namespace PersonalApp.Controllers
{
    public class MoneyManagementController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        // GET: MoneyManagement

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMoneyManagement(MoneyManagement moneyManagement)
        {
                ViewBag.head = "All Months Expenses";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * From DayList", con);
                    {
                        DataSet dataSet = new DataSet();
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataSet);
                        List<AllExpenses> list = new List<AllExpenses>();
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            AllExpenses todo = new AllExpenses();
                            todo.id = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Id"]);
                            todo.Categories = dataSet.Tables[0].Rows[i]["Category"].ToString();
                            todo.Amount = dataSet.Tables[0].Rows[i]["Amount"].ToString();
                            todo.Description = dataSet.Tables[0].Rows[i]["Descriptions"].ToString();
                            todo.Date = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["Dates"]);
                            todo.CategoryName = dataSet.Tables[0].Rows[i]["ImageName"].ToString();

                            list.Add(todo);
                        }
                    moneyManagement.AllExpensesList = list;
                    }
                }
                return View(moneyManagement);
            }


      
    }
}