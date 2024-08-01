using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class MonthExpense
    {
        public List<MonthExpenseClass> monthExpenseList = new List<MonthExpenseClass>();
        public class MonthExpenseClass
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }

            public string Description { get; set; }

            public string Amount { get; set; }

            public DateTime DateTime { get; set; }

            public string IamgeName { get; set; }
        }

    }
}