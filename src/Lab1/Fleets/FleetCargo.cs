using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public sealed class FleetCargo
{
    public FleetCargo(IEnumerable<CargoHold> holds)
    {
        ArgumentNullException.ThrowIfNull(holds);

        Holds = holds.ToArray();
    }

    public IReadOnlyCollection<CargoHold> Holds { get; }

    public Volume FreeSpace => new Volume(Holds.Sum(hold => hold.FreeSpace.CubicMeters));

    public Volume LoadedVolume => new Volume(Holds.Sum(hold => hold.LoadedVolume.CubicMeters));
}
