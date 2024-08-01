using PersonalApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PersonalApp.Models.Category;
using static PersonalApp.Models.Category.Image;

namespace PersonalApp.Controllers
{
    public class CategoryController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Categories(Category category)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO DayList (Category, Descriptions, Amount, Dates, ImageName, TagName) " +
                                   "VALUES (@Category, @Descriptions, @Amount, @Dates, @ImageName, @TagName)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Category", category.Categories);
                    cmd.Parameters.AddWithValue("@Descriptions", category.Description);
                    cmd.Parameters.AddWithValue("@Amount", category.Amount);
                    cmd.Parameters.AddWithValue("@Dates", category.Date);
                    cmd.Parameters.AddWithValue("@ImageName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@TagName", category.TagName);

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("TodoList","TodoList");
            }
            // Handle validation errors here
            return RedirectToAction("TodoList", "TodoList");
        }


        public ActionResult Categories()
        {

            ViewBag.head = "Add Categories";
            Category category = new Category();
            List<Category.Image> images = new List<Category.Image>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, ImageUrl,ImageName FROM ExtraImages";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    images.Add(new Category.Image
                    {
                        Id = (int)reader["Id"],
                        ImageUrl = (string)reader["ImageUrl"],
                        ImageName = (string)reader["ImageName"]
                    });
                }
            }



            List<Category.DefaultIcon> defaultIconsList = new List<Category.DefaultIcon>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, ImageUrl,ImageName FROM DefaultIcons";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    defaultIconsList.Add(new Category.DefaultIcon
                    {
                        Id = (int)reader["Id"],
                        ImageUrl = (string)reader["ImageUrl"], 
                        ImageName = (string)reader["ImageName"]
                    });
                }
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Tags";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                List<TagNameClass> tagNameClassesList = new List<TagNameClass>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TagNameClass tagNameClass = new TagNameClass();
                    tagNameClass.TagName = ds.Tables[0].Rows[i]["TagName"].ToString();
                    tagNameClassesList.Add(tagNameClass);
                }
                category.tagNameList = tagNameClassesList;
            }

                category.defaultIconsList = defaultIconsList;
            category.imagesList = images;

            return View(category);
        }


        public ActionResult ExtraCategories()
        {
            return View();
        }
    }
}