using Microsoft.EntityFrameworkCore;
using Portal.Usuario.Core.Entities;

namespace Portal.Usuario.Infrastructure.DatabaseServices.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
