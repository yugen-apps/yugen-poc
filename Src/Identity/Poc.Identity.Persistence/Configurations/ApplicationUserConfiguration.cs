using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yugen.Common.Identity.Domain.Entities;

namespace Poc.Identity.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(450);

        builder.HasOne(x => x.Tenant)
               .WithMany()
               .HasForeignKey(u => u.TenantId);

        builder.Navigation(e => e.Tenant)
               .AutoInclude();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
               .WithOne(e => e.User)
               .HasForeignKey(ul => ul.UserId)
               .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
               .WithOne(e => e.User)
               .HasForeignKey(ut => ut.UserId)
               .IsRequired();

        // Each User can have many UserClaims
        builder.HasMany(e => e.UserClaims)
               .WithOne(e => e.User)
               .HasForeignKey(uc => uc.UserId)
               .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();
    }
}
