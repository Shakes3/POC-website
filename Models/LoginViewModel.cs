using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace POC3.Models
{
    public class LoginViewModel
    {
        public string Email{get;set;}
        public string Password{get;set;}
    }
}