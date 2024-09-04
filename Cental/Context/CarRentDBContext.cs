using Cental.Models;
using Microsoft.EntityFrameworkCore;

namespace Cental.Context
{
    public class CarRentDBContext : DbContext
    {
        public CarRentDBContext(DbContextOptions<CarRentDBContext> options) : base(options) { }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogsTags> BlogsTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Author)
                .WithMany(a=>a.Blogs)
                .HasForeignKey(b=>b.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
