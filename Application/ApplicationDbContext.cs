using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using POC3.Models;

namespace POC3.Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Computer> Computers {get; set; }
        public DbSet<Company> Companies {get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            if (!optionbuilder.IsConfigured)
            {
                optionbuilder.UseSqlServer("Server=G40XMS3\\SQLSERVER2022 ; Database= Demo1DB ; Trusted_Connection=True; TrustServerCertificate=True;");
            }
        }
    }

}


