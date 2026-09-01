using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yugen.Common.Identity.Domain.Entities;

namespace Poc.Identity.Persistence.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasIndex(x => x.NormalizedName)
               .IsUnique();

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany(e => e.RoleClaims)
               .WithOne(e => e.Role)
               .HasForeignKey(rc => rc.RoleId)
               .IsRequired();
    }
}
