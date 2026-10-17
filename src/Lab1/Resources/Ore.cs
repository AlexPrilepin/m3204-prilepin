using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Resources;

public sealed class Ore
{
    private readonly IReadOnlyCollection<MineralAmount> _portionYield;

    public Ore(string name, Volume unitVolume, IReadOnlyCollection<MineralAmount> portionYield)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitVolume.CubicMeters, nameof(unitVolume));
        ArgumentNullException.ThrowIfNull(portionYield);

        if (portionYield.Count == 0)
        {
            throw new ArgumentException("Ore must yield at least one mineral.", nameof(portionYield));
        }

        Name = name;
        UnitVolume = unitVolume;
        _portionYield = portionYield.ToArray();
    }

    public string Name { get; }

    public Volume UnitVolume { get; }

    public IEnumerable<Mineral> Minerals => _portionYield.Select(amount => amount.Mineral);

    public int CountUnits(Volume volume)
    {
        return decimal.ToInt32(decimal.Floor(volume.CubicMeters / UnitVolume.CubicMeters));
    }

    public IReadOnlyCollection<MineralAmount> CalculateYield(int portions)
    {
        return _portionYield
            .Select(amount => amount.MultiplyBy(portions))
            .ToArray();
    }
}
