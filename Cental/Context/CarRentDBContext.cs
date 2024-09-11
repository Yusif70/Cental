using Cental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cental.Context
{
    public class CarRentDBContext : IdentityDbContext<AppUser>
    {
        public CarRentDBContext(DbContextOptions<CarRentDBContext> options) : base(options) { }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogsTags> BlogsTags { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique(true);
            modelBuilder.Entity<Category>()
                .HasIndex(c=>c.Name)
                .IsUnique(true);
        }
    }
}
