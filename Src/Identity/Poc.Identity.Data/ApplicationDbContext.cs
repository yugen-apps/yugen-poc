using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain;
using Yugen.Common.Data;
using Yugen.Common.Data.Entities;

namespace Poc.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}
	
	public new DbSet<TEntity> Set<TEntity>()
		where TEntity : BaseEntity => base.Set<TEntity>();

	public ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
	{
		throw new NotImplementedException();
	}

	public Task<bool> CanConnectAsync()
	{
		throw new NotImplementedException();
	}

	public Task<int> DeleteAsync<TEntity>(int id) where TEntity : BaseEntity
	{
		throw new NotImplementedException();
	}

	public Task<TEntity?> GetBydIdAsync<TEntity>(int id) where TEntity : BaseEntity
	{
		throw new NotImplementedException();
	}
}
