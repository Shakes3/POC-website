using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POC3.Application;
using POC3.Models;

namespace POC3.Controllers
{
    // [Route("[controller]")]
    public class ComputerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSystem()
        {
            var data = _context.Computers.Include(c => c.Company).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult AddSystem()
        {
            // return View();
            var companies = _context.Companies.ToList();
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        public IActionResult AddSystem(Computer system)
        {
            // _context.Computers.Add(system);
            // _context.SaveChanges();
            // return RedirectToAction("GetSystem");
            // if (ModelState.IsValid)
            // {
                _context.Computers.Add(system);
                _context.SaveChanges();
                return RedirectToAction("GetSystem");
            // }

            ViewBag.Companies = _context.Companies.ToList();
            return View(system);
        }

        [HttpPost]   // action method to delete 
        public IActionResult DeleteSystem(int ComputerId)
        {
            var data = _context.Computers.Find(ComputerId);
            if(data != null)
            {
                _context.Computers.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("GetSystem");
            }
            return View(data);
        }
    }
}