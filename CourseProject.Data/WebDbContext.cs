using CourseProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data
{
    public class WebDbContext : DbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<UserData> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .HasIndex(m => m.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
