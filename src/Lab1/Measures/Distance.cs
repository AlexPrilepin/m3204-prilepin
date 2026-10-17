namespace Itmo.ObjectOrientedProgramming.Lab1.Measures;

public readonly record struct Distance
{
    public Distance(decimal astronomicalUnits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(astronomicalUnits);

        AstronomicalUnits = astronomicalUnits;
    }

    public decimal AstronomicalUnits { get; }

    public static Duration operator /(Distance distance, Speed speed)
    {
        ArgumentNullException.ThrowIfNull(speed);

        return new Duration(distance.AstronomicalUnits / speed.AstronomicalUnitsPerHour);
    }
}
