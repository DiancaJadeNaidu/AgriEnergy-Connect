// Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10261874_PROG7311.Models;

namespace ST10261874_PROG7311.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

//databases
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
