using FinalAutoFillServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalAutoFillServer.Data.EntityConfiguration
{
    public class UserTypeEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId)
            .ValueGeneratedOnAdd()
            .IsRequired();
            entity.Property(e => e.UserName)
            .HasMaxLength(20)
            .IsRequired();
            entity.Property(e => e.Role)
            .IsRequired();

            entity.Property(e => e.Name)
            .HasMaxLength(50);
            entity.Property(e => e.Phone)
            .HasMaxLength(15);
        }
    }
}
