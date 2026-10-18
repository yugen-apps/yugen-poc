using System;

namespace Yugen.Common.Data.Entities;

/// <summary>
/// Represents the base class that all entities derive from.
/// </summary>
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    /// <summary>
    /// Gets the entity identifier.
    /// </summary>
    public Guid Id { get; private set; }

    //public DateTime? CreatedAt { get; set; }
    //public string? CreatedById { get; set; }
    //public DateTime? ModifiedAt { get; set; }
    //public string? ModifiedById { get; set; }
    //public DateTime? DeletedAt { get; set; }
    //public string? DeletedById { get; set; }

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        if (a is null &&
            b is null)
        {
            return true;
        }

        if (a is null ||
            b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity? a, BaseEntity? b) => !(a == b);

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