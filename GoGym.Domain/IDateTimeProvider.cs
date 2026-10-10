namespace GoGym.Domain;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}