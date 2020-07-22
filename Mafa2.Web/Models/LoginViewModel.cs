using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class LoginViewModel
    {
        public int IDUsera { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public int IDUloge { get; set; }
    }
}