using Microsoft.EntityFrameworkCore;
 
namespace Network.Models
{
    public class NTContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public NTContext(DbContextOptions<NTContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Connection> connections {get; set;}
        public DbSet<Invitation> invitations {get; set;}
    }
}