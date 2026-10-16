namespace GoGym.Domain.Common;

public abstract class ValueObject
{
    public abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return ((ValueObject)obj).GetEqualityComponents()
                            .SequenceEqual(GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach(var component in GetEqualityComponents())
        {
            hashCode.Add(component?.GetHashCode() ?? 0);
        }

        return hashCode.ToHashCode();
    }

    protected ValueObject() {}
}