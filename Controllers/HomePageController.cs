using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using PersonalApp.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using static PersonalApp.Models.HomePageLabelY;
using Newtonsoft.Json.Linq;

namespace PersonalApp.Controllers
{
    public class HomePageController : Controller
    {

        #region Objects and Variables

        string connectionString = ConfigurationManager.ConnectionStrings["PersonalApp"].ConnectionString;
        public int TotalExpenseAmount = 0, totalBalanceAmount = 0, CurrentMonthSalary = 0;
        public int food = 0, home = 0, travel = 0, others = 0, electronics = 0, medical = 0, shopping = 0;
        int JanSalary = 0, FebSalary = 0, MarSalary = 0, AprSalary = 0, MaySalary = 0, JunSalary = 0, JulSalary = 0, AugSalary = 0, SepSalary = 0, OctSalary = 0, NovSalary = 0, DecSalary = 0;
        int JanExpense = 0, FebExpense = 0, MarExpense = 0, AprExpense = 0, MayExpense = 0, JunExpense = 0, JulExpense = 0, AugExpense = 0, SepExpense = 0, OctExpense = 0, NovExpense = 0, DecExpense = 0;

        public bool flag = false;
        #endregion
        // GET: HomePage

        #region Helping Methods

        public class SalaryDateChecker
        {
            public static bool IsCurrentMonth(DateTime dateToCheck)
            {
                DateTime currentDate = DateTime.Now;
                return dateToCheck.Month == currentDate.Month && dateToCheck.Year == currentDate.Year;
            }
        }


        static DateTime GetLastDateOfMonth(int year, int month)
        {
            // Start with the first day of the next month, then subtract one day
            DateTime firstDayOfNextMonth = new DateTime(year, month, 1).AddMonths(1);
            DateTime lastDateOfMonth = firstDayOfNextMonth.AddDays(-1);
            return lastDateOfMonth;
        }


        #endregion

        #region HomePage ActionResult

        public ActionResult HomePage()
        {
            ViewBag.head = "Dashboard";


            // *****************Total Current Month Salary*******************************

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "Select * from Salary";
                SqlCommand cmd = new SqlCommand(query, conn);

                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                SqlDataReader rdr = cmd.ExecuteReader();

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime salaryDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["SalaryDate"]);
                    bool isCurrentMonth = SalaryDateChecker.IsCurrentMonth(salaryDate);

                    if (isCurrentMonth)
                    {
                        CurrentMonthSalary += Convert.ToInt32(dataSet.Tables[0].Rows[i]["SalaryAmount"]);
                    }
                }

                ViewBag.Salary = CurrentMonthSalary;
            }

            //*********************Total current Month Expense******************************

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from DayList";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                List<TotalExpense> totalExpensesList = new List<TotalExpense>();

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime salaryDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["Dates"]);
                    bool isCurrentMonth = SalaryDateChecker.IsCurrentMonth(salaryDate);

                    if (isCurrentMonth)
                    {
                        TotalExpenseAmount += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                    }
                }
            }

            TotalExpense totalExpenses = new TotalExpense();

            totalExpenses.totalAmountExpenses = TotalExpenseAmount;


            if(ViewBag.Salary < TotalExpenseAmount)
            {
                ViewBag.Balance = ViewBag.Salary - TotalExpenseAmount;
            }


            //************************* Pie Chart Values*************************************

            List<HomePageLabelY> dataPoints = new List<HomePageLabelY>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from DayList";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                DateTime currDate = DateTime.Now;
                int currentMonth = currDate.Month;

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime ExpenseDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["Dates"]);
                    if (ExpenseDate.Month == currentMonth)
                    {

                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Food")
                        {
                            food += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }

                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Home")
                        {
                            home += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }

                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Travel")
                        {
                            travel += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }

                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Others")
                        {
                            others += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }
                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Electronics")
                        {
                            electronics += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }
                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Medical")
                        {
                            medical += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }
                        if (dataSet.Tables[0].Rows[i]["TagName"].ToString() == "Shopping")
                        {
                            shopping += Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);
                        }
                    }
                }
            }

            //(value / total value)×100 %
            dataPoints.Add(new HomePageLabelY("Food", Math.Round(((double)food / TotalExpenseAmount) * 100, 2)));
            dataPoints.Add(new HomePageLabelY("Home", Math.Round(((double)home / TotalExpenseAmount) * 100, 2)));
            dataPoints.Add(new HomePageLabelY("Travel", Math.Round(((double)travel / TotalExpenseAmount) * 100, 2)));
            dataPoints.Add(new HomePageLabelY("Others", Math.Round(((double)others / TotalExpenseAmount) * 100, 2)));
            dataPoints.Add(new HomePageLabelY("Electronics", Math.Round(((double) electronics / TotalExpenseAmount) * 100,2)));
            dataPoints.Add(new HomePageLabelY("Medical", Math.Round(((double)medical / TotalExpenseAmount) * 100, 2)));
            dataPoints.Add(new HomePageLabelY("Shopping", Math.Round(((double)shopping / TotalExpenseAmount) * 100, 2)));



            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            //*******************************Bar Chart Values*******************************************


            List<DataPoint> dataPointss = new List<DataPoint>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Salary";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);


                DateTime curDate = DateTime.Now;
                int currentYear = curDate.Year;

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime salaryDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["SalaryDate"]);
                    int salaryAmount = Convert.ToInt32(dataSet.Tables[0].Rows[i]["SalaryAmount"]);
                    long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                    if (salaryDate.Year == currentYear)
                    {
                        if (salaryDate.Month == 1)
                        {
                            JanSalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 2)
                        {
                            FebSalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 3)
                        {
                            MarSalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 4)
                        {
                            AprSalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 5)
                        {
                            MaySalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 6)
                        {
                            JunSalary += salaryAmount;
                            
                        }
                        else if (salaryDate.Month == 7)
                        {
                            JulSalary += salaryAmount;
                           
                        }
                        else if(salaryDate.Month == 8)
                        {
                            AugSalary += salaryAmount;
                           
                        }else if (salaryDate.Month == 9)
                        {
                            SepSalary += salaryAmount;
                            
                        }else if(salaryDate.Month == 10)
                        {
                            OctSalary += salaryAmount;
                            
                        }else if(salaryDate.Month == 11)
                        {
                            NovSalary += salaryAmount;
                           
                        }else if(salaryDate.Month== 12)
                        {
                            DecSalary += salaryAmount;
                            
                        }
                    }
                }
                dataPointss.Add(new DataPoint(1483209000000, JanSalary));
                dataPointss.Add(new DataPoint(1485887400000, FebSalary));
                dataPointss.Add(new DataPoint(1488306600000, MarSalary));
                dataPointss.Add(new DataPoint(1490985000000, AprSalary));
                dataPointss.Add(new DataPoint(1493577000000, MaySalary));
                dataPointss.Add(new DataPoint(1496255400000, JunSalary));
                dataPointss.Add(new DataPoint(1498847400000, JulSalary));
                dataPointss.Add(new DataPoint(1501525800000, AugSalary));
                dataPointss.Add(new DataPoint(1504204200000, SepSalary));
                dataPointss.Add(new DataPoint(1506796200000, OctSalary));
                dataPointss.Add(new DataPoint(1509474600000, NovSalary));
                dataPointss.Add(new DataPoint(1512066600000, DecSalary));

            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPointss);
 
			dataPointss = new List<DataPoint>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from DayList";
                SqlCommand cmd = new SqlCommand(query, con);

                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);


                DateTime currentDates = DateTime.Now;
                int currentYear = currentDates.Year;

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DateTime ExpenseDate = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["Dates"]);
                    int ExpenseAmount = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Amount"]);

                    if (ExpenseDate.Year == currentYear)
                    {
                        if (ExpenseDate.Month == 1)
                        {
                            JanExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 2)
                        {
                            FebExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 3)
                        {
                            MarExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 4)
                        {
                            AprExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 5)
                        {
                            MayExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 6)
                        {
                            JunExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 7)
                        {
                            JulExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 8)
                        {
                            AugExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 9)
                        {
                            SepExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 10)
                        {
                            OctExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 11)
                        {
                            NovExpense += ExpenseAmount;
                            
                        }
                        else if (ExpenseDate.Month == 12)
                        {
                            DecExpense += ExpenseAmount;
                           
                        }
                    }
                }

                dataPointss.Add(new DataPoint(1483209000000, JanExpense));
                dataPointss.Add(new DataPoint(1485887400000, FebExpense));
                dataPointss.Add(new DataPoint(1488306600000, MarExpense));
                dataPointss.Add(new DataPoint(1490985000000, AprExpense));
                dataPointss.Add(new DataPoint(1493577000000, MayExpense));
                dataPointss.Add(new DataPoint(1496255400000, JunExpense));
                dataPointss.Add(new DataPoint(1498847400000, JulExpense));
                dataPointss.Add(new DataPoint(1501525800000, AugExpense));
                dataPointss.Add(new DataPoint(1504204200000, SepExpense));
                dataPointss.Add(new DataPoint(1506796200000, OctExpense));
                dataPointss.Add(new DataPoint(1509474600000, NovExpense));
                dataPointss.Add(new DataPoint(1512066600000, DecExpense));
            }
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPointss);
 
			dataPointss = new List<DataPoint>();

			dataPointss.Add(new DataPoint(1483209000000, (JanSalary - JanExpense)));
			dataPointss.Add(new DataPoint(1485887400000, (FebSalary - FebExpense)));
			dataPointss.Add(new DataPoint(1488306600000, (MarSalary - MarExpense)));
			dataPointss.Add(new DataPoint(1490985000000, (AprSalary - AprExpense)));
			dataPointss.Add(new DataPoint(1493577000000, (MaySalary - MayExpense)));
			dataPointss.Add(new DataPoint(1496255400000, (JunSalary - JunExpense)));
			dataPointss.Add(new DataPoint(1498847400000, (JulSalary - JulExpense)));
			dataPointss.Add(new DataPoint(1501525800000, (AugSalary - AugExpense)));
			dataPointss.Add(new DataPoint(1504204200000, (SepSalary - SepExpense)));
			dataPointss.Add(new DataPoint(1506796200000, (OctSalary - OctExpense)));
			dataPointss.Add(new DataPoint(1509474600000, (NovSalary - NovExpense)));
			dataPointss.Add(new DataPoint(1512066600000, (DecSalary - DecExpense)));
 
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPointss);



            DateTime currentDate = DateTime.Now;
            int year = currentDate.Year;
            int month = currentDate.Month;


            DateTime lastDateOfMonth = GetLastDateOfMonth(year, month);

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string qury = "select datemonth from TotalBalance";
                SqlCommand cmmd  = new SqlCommand(qury, con);
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmmd);
                adapter.Fill(dataSet);
                DateTime Temp = DateTime.Now;
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    Temp = Convert.ToDateTime(dataSet.Tables[0].Rows[i]["datemonth"]);
                }


                if (currentDate.Date == lastDateOfMonth.Date && currentDate.Date != Convert.ToDateTime(Temp))
                {
                    ViewBag.Salary = "0";
                    ViewBag.Expense = "0";
                    string query = "Insert into TotalBalance (TotalBalance, datemonth) values (@TotalBalance, @datemonth)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@TotalBalance", (CurrentMonthSalary - TotalExpenseAmount));
                    cmd.Parameters.AddWithValue("@datemonth", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "Select * from TotalBalance";
                    SqlCommand cmd = new SqlCommand(query, con);

                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataSet);

                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        totalBalanceAmount += Convert.ToInt32(dataSet.Tables[0].Rows[i]["TotalBalance"]);
                    }

                    TotalExpense totalExpensess = new TotalExpense();

                    totalExpensess.totalBalanceAmount = totalBalanceAmount;

                    ViewBag.TotalAmountBalance = totalBalanceAmount;

                }
            


            return View(totalExpenses);
        }

        #endregion

    }
}
