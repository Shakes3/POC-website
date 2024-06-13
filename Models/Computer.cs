using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC3.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }
        public string ComputerName { get; set; }
        public string ComputerNumber { get; set; }
        public string ComputerStatus { get; set; }
        public string ComputerUserName { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}