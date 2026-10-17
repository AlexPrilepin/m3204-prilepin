namespace Itmo.ObjectOrientedProgramming.Lab1.Measures;

public readonly record struct Volume : IComparable<Volume>
{
    public Volume(decimal cubicMeters)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(cubicMeters);

        CubicMeters = cubicMeters;
    }

    public decimal CubicMeters { get; }

    public bool IsZero => CubicMeters == 0;

    public static Volume operator +(Volume left, Volume right)
    {
        return new Volume(left.CubicMeters + right.CubicMeters);
    }

    public static Volume operator -(Volume left, Volume right)
    {
        return new Volume(left.CubicMeters - right.CubicMeters);
    }

    public static bool operator <(Volume left, Volume right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(Volume left, Volume right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(Volume left, Volume right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(Volume left, Volume right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int CompareTo(Volume other)
    {
        return CubicMeters.CompareTo(other.CubicMeters);
    }
}
