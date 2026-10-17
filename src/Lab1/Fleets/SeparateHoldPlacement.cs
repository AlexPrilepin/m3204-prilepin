using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public sealed class SeparateHoldPlacement : IPlacementStrategy
{
    public bool TryPlace(CargoHold minerHold, FleetCargo cargo, Volume volume)
    {
        ArgumentNullException.ThrowIfNull(minerHold);

        if (minerHold.FreeSpace < volume)
        {
            return false;
        }

        minerHold.Fill(volume);

        return true;
    }
}
