using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POC3.Application;

namespace POC3.Controllers
{
    // [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}