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
    }
}
