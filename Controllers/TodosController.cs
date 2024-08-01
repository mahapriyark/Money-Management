using PersonalApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PersonalApp.Models.Todos;

namespace PersonalApp.Controllers
{
    public class TodoListController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        // GET: TodoList
        public ActionResult Index()
        {
            return View();
        }

        public class DateHelper
        {
            public static bool IsCurrentMonth(DateTime dateToCheck)
            {
                DateTime currentDate = DateTime.Now;
                return dateToCheck.Month == currentDate.Month && dateToCheck.Year == currentDate.Year;
            }
        }

        public ActionResult TodoList(Todos todoclass)
        {
            ViewBag.head = "Add Expenses";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DayList", con);
                DataSet dataSet = new DataSet();
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                List<Todo> todos = new List<Todo>();

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime date = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["Dates"]);
                    if (DateHelper.IsCurrentMonth(date))
                    {
                        Todo todo = new Todo
                        {
                            id = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Id"]),
                            Categories = dataSet.Tables[0].Rows[i]["Category"].ToString(),
                            Amount = dataSet.Tables[0].Rows[i]["Amount"].ToString(),
                            Description = dataSet.Tables[0].Rows[i]["Descriptions"].ToString(),
                            Date = date,
                            CategoryName = dataSet.Tables[0].Rows[i]["ImageName"].ToString()
                        };

                        todos.Add(todo);
                    }
                }

                todoclass.todosList = todos;
            }

            return View(todoclass);
        }


        [HttpPost]
        public ActionResult EditTodoItem(Todo todo)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE DayList SET Category = @Category, Dates = @Dates, Descriptions = @Descriptions, Amount = @Amount WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", todo.id);
                cmd.Parameters.AddWithValue("@Category", todo.Categories);
                cmd.Parameters.AddWithValue("@Dates", todo.Date);
                cmd.Parameters.AddWithValue("@Descriptions", todo.Description);
                cmd.Parameters.AddWithValue("@Amount", todo.Amount);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("TodoList");
        }


        [HttpPost]
        public ActionResult DeleteTodoItem(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "DELETE FROM DayList WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

           return RedirectToAction("TodoList");
        }

    }
}