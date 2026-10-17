namespace Itmo.ObjectOrientedProgramming.Lab1.Stations;

public readonly record struct OreStorage
{
    private const int UnitsPerPortion = 100;

    public OreStorage(int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(units);

        Units = units;
    }

    public int Units { get; }

    public int FullPortions => Units / UnitsPerPortion;

    public OreStorage Store(int units)
    {
        return new OreStorage(Units + units);
    }

    public OreStorage RemoveFullPortions()
    {
        return new OreStorage(Units % UnitsPerPortion);
    }
}
