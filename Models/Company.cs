using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC3.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string CompanyStatus { get; set; }
        public List<Computer>? Computers { get; set; }

    }
}