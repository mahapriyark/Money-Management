using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PersonalApp.Models
{
    [DataContract]
    public class HomePageLabelY
    {
        public HomePageLabelY(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
        [DataMember(Name = "label")]
        public string Label = "";

        [DataMember(Name = "y")]
        public Nullable<double> Y = null;


        public List<TotalExpense> totalExpensesList = new List<TotalExpense>();

        public class TotalExpense
        {
            public int totalAmountExpenses { get; set; }

            public int totalBalanceAmount { get; set; }

        }

    }

        [DataContract]
        public class DataPoint
        {
            public DataPoint(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public Nullable<double> X = null;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
}
