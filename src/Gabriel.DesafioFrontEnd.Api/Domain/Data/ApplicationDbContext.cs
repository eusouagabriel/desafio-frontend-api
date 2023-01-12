using Gabriel.DesafioFrontEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gabriel.DesafioFrontEnd.Api.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<Camera> Camera { get; set; } = default!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseInMemoryDatabase(databaseName: "DesafioFrontendDb");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Cameras)
                .WithOne(cus => cus.Customer);
        }

    }
}
