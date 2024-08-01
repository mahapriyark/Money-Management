using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class MoneyManagement
    {
        public List<AllExpenses> AllExpensesList= new List<AllExpenses>();

        public class AllExpenses
        {
            public int id { get; set; }
            public string Description { get; set; }

            public string Categories { get; set; }

            public string CategoryName { get; set; }

            public string Amount { get; set; }

            public DateTime Date { get; set; }
        }
    }
}