using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;

namespace Itmo.ObjectOrientedProgramming.Lab1.Calculation;

public sealed class ContractReport
{
    public ContractReport(
        Duration workingTime,
        IEnumerable<Volume> tripVolumes,
        IEnumerable<MineralAmount> minerals,
        int remainingOreUnits,
        decimal revenue,
        decimal rent,
        decimal tax)
    {
        ArgumentNullException.ThrowIfNull(tripVolumes);
        ArgumentNullException.ThrowIfNull(minerals);
        ArgumentOutOfRangeException.ThrowIfNegative(remainingOreUnits);

        WorkingTime = workingTime;
        TripVolumes = tripVolumes.ToArray();
        Minerals = minerals.ToArray();
        RemainingOreUnits = remainingOreUnits;
        Revenue = revenue;
        Rent = rent;
        Tax = tax;
        MinedVolume = new Volume(TripVolumes.Sum(volume => volume.CubicMeters));
    }

    public Duration WorkingTime { get; }

    public IReadOnlyCollection<Volume> TripVolumes { get; }

    public int TripCount => TripVolumes.Count;

    public Volume MinedVolume { get; }

    public IReadOnlyCollection<MineralAmount> Minerals { get; }

    public int RemainingOreUnits { get; }

    public decimal Revenue { get; }

    public decimal Rent { get; }

    public decimal Tax { get; }

    public decimal Profit => Revenue - Rent - Tax;
}
