using ProleitPocBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ProleitPocBackend.Data
{
    public class ProleitPocBackendDbContext : DbContext
    {
        public ProleitPocBackendDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Device> devices { get; set; }
    }
}