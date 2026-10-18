using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yugen.Common.Blazor.Account.Domain.Entities;

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

    //public static void Map(EntityTypeBuilder<ApplicationUser> entity, Guid currentTenantId)
    //{
    //    entity.HasKey(x => x.Id);

    //    entity.HasQueryFilter(x => !x.IsDeleted);

    //    entity.Property(e => e.Id)
    //          .ValueGeneratedOnAdd();

    //    entity.Property(x => x.Title)
    //        .IsRequired()
    //        .HasMaxLength(512);

    //    entity.HasMany<Category>()
    //          .WithOne()
    //          .HasForeignKey(x => x.ParentId);

    //    entity.HasIndex(x => x.Title);
    //}
}
