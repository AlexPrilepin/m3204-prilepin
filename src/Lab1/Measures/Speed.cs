namespace Itmo.ObjectOrientedProgramming.Lab1.Measures;

public sealed record Speed
{
    public Speed(decimal astronomicalUnitsPerHour)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(astronomicalUnitsPerHour);

        AstronomicalUnitsPerHour = astronomicalUnitsPerHour;
    }

    public decimal AstronomicalUnitsPerHour { get; }
}
