using authMicroservice.Entities;
using Microsoft.EntityFrameworkCore;


namespace authMicroservice.Database
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options): DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
