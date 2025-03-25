using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using src.Models;

namespace src.Data
{
    public class BasicStructureDbContext
    {
        public class ProjectDbContext : DbContext
        {
            public DbSet<User> Users { get; set; }

            public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
                : base(options)
            { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<User>()
                    .HasKey(u => u.Id);
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseLazyLoadingProxies();
            }
        }
    }
}
