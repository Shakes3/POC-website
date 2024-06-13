using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POC3.Application;
using POC3.Models;

namespace POC3.Controllers
{
    // [Route("[controller]")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCompany()
        {
            var data = _context.Companies.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddCompany(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
            return RedirectToAction("GetCompany") ;
        }

        [HttpPost]
        public IActionResult DeleteCompany(int CompanyId)
        {
            var data = _context.Companies.Find(CompanyId);
            if(data != null)
            {
                _context.Companies.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("GetCompany");
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult EditCompany(Company company)
        {
            // if (company.CompanyName != null && company.CompanyStatus != null)
            if(ModelState.IsValid)
            {
                _context.Companies.Update(company);
                _context.SaveChanges();
                return RedirectToAction("GetCompany");
            }
            return View(company);
        }
    }
}