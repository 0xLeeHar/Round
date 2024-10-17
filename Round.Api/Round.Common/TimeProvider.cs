namespace Round.Common;

public interface ITimeProvider
{
    DateTime GetUtcNow();
}

public class TimeProvider : ITimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}