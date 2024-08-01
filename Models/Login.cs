using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int User_Priority { get; set; }
    }

    public class Register
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}