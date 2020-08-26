using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaySix.Models;
using Microsoft.EntityFrameworkCore;

namespace DaySix.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}
