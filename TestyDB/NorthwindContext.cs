using Microsoft.EntityFrameworkCore;

namespace TestyDB
{
    public class NorthwindContext : DbContext
    {
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;Database=northwind;Uid=postgres;Pwd=top123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}