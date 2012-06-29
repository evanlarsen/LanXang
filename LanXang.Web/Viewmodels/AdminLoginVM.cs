using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanXang.Web.Viewmodels
{
    public class AdminLoginVM
    {
        public string Password { get; set; }
        public bool LoginFailed { get; set; }
    }
}