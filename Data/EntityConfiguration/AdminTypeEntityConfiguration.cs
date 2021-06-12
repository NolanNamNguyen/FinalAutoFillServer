using FinalAutoFillServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalAutoFillServer.Data.EntityConfiguration
{
    public class AdminTypeEntityConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> entity)
        {
            entity.HasKey(e => e.AdminId);
            entity.Property(e => e.AdminId)
            .ValueGeneratedOnAdd()
            .IsRequired();
        }
    }
}
