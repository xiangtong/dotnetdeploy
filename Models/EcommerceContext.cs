using Microsoft.EntityFrameworkCore;
 
namespace Ecommerce.Models
{
    public class ECContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ECContext(DbContextOptions<ECContext> options) : base(options) { }
        public DbSet<Product> products { get; set; }
        public DbSet<Customer> customers {get; set;}
        public DbSet<Order> orders {get; set;}
    }
}