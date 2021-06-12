using FinalAutoFillServer.Data.EntityConfiguration;
using FinalAutoFillServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinalAutoFillServer.Data
{
    public class AutoFillContext : DbContext
    {
        public AutoFillContext(DbContextOptions<AutoFillContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdminTypeEntityConfiguration());
        }
    }
}
