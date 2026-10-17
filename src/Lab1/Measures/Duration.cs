namespace Itmo.ObjectOrientedProgramming.Lab1.Measures;

public readonly record struct Duration : IComparable<Duration>
{
    public Duration(decimal hours)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(hours);

        Hours = hours;
    }

    public decimal Hours { get; }

    public static Duration operator +(Duration left, Duration right)
    {
        return new Duration(left.Hours + right.Hours);
    }

    public static bool operator <(Duration left, Duration right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(Duration left, Duration right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(Duration left, Duration right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(Duration left, Duration right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int CompareTo(Duration other)
    {
        return Hours.CompareTo(other.Hours);
    }
}
