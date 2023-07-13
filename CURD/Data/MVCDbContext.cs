using CURD.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CURD.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Employe> Employe { get; set; }
    }
}
