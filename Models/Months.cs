using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class Months
    {

        public List<MonthsClass> monthsClassesList = new List<MonthsClass>();   

        public class MonthsClass
        {
            public string UserName { get; set; }

            public DateTime SalaryDate { get; set; }

            public int SalaryAmt { get; set; }

            public string CompanyName { get; set; }
        }

    }
}