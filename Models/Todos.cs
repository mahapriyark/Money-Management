using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class Todos
    {
        public List<Todo> todosList = new List<Todo>();

        public class Todo
        {
            public int id {  get; set; }
            public string Description { get; set; }

            public string Categories { get; set; }

            public string CategoryName { get; set; }

            public string Amount { get; set; }

            public DateTime Date { get; set; }
        }

    }
}