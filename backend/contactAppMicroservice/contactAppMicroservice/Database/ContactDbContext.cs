using contactAppMicroservice.Entities;
using Microsoft.EntityFrameworkCore;

namespace contactAppMicroservice.Database
{
    public class ContactDbContext(DbContextOptions<ContactDbContext> options): DbContext(options)
    {
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();
                
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Subcategory)
                .WithMany()
                .HasForeignKey(c => c.SubcategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Subcategory>()
                .HasOne(s => s.Category)
                .WithMany()
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
