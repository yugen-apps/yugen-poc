using Ef.Poc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ef.Poc.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Users => Set<Category>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Map(modelBuilder.Entity<Category>());
    }

    private static void Map(EntityTypeBuilder<Category> entity)
    {
        entity.HasKey(x => x.Id);

        //entity.Property(e => e.Id).ValueGeneratedOnAdd();

        //entity.HasQueryFilter(x => !x.IsDeleted);

        entity.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(512);

        //entity.HasMany<Category>().WithOne().HasForeignKey(x => x.ParentId);

        entity.HasIndex(x => x.Title);
    }
}