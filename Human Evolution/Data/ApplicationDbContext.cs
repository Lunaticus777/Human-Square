using Microsoft.EntityFrameworkCore;
using Human_Evolution.Models;
using Microsoft.VisualBasic;


namespace Human_Evolution.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

    }
}
