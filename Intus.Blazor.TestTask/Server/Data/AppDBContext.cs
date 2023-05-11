using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intus.Blazor.TestTask.Server.Models;
using Intus.Blazor.TestTask.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Intus.Blazor.TestTask.Server.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Windows> Windows { get; set; }
        public DbSet<Elements> Elements { get; set; }
        public DbSet<OrderWindow> OrderWindow { get; set; }
        public DbSet<OrderWindowElement> OrderWindowElement { get; set; }

    }
}
