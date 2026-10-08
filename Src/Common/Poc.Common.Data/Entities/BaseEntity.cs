using System;

namespace Poc.Common.Data.Entities;

/// <summary>
/// Represents the base class that all entities derive from.
/// </summary>
public abstract class BaseEntity : IEquatable<BaseEntity>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseEntity"/> class.
	/// </summary>
	/// <param name="id">The entity identifier.</param>
	protected BaseEntity(int id)
	{
		Id = id;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseEntity"/> class.
	/// </summary>
	/// <remarks>
	/// Required by EF Core.
	/// </remarks>
	protected BaseEntity()
	{
	}

	/// <summary>
	/// Gets the entity identifier.
	/// </summary>
	public int Id { get; private set; }
	// public DateTime CreationTime { get; set; }
	// public DateTime? UpdatedAt { get; set; }

	public static bool operator ==(BaseEntity a, BaseEntity b)
	{
		if (a is null && b is null)
		{
			return true;
		}

		if (a is null || b is null)
		{
			return false;
		}

		return a.Equals(b);
	}

	public static bool operator !=(BaseEntity a, BaseEntity b) => !(a == b);

	/// <inheritdoc />
	public bool Equals(BaseEntity? other)
	{
		if (other is null)
		{
			return false;
		}

		return ReferenceEquals(this, other) || Id == other.Id;
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		if (!(obj is BaseEntity other))
		{
			return false;
		}

		return Id == other.Id;
	}

	/// <inheritdoc />
	public override int GetHashCode() => Id.GetHashCode() * 41;

}