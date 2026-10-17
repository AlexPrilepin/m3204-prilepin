using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public sealed class SharedHoldPlacement : IPlacementStrategy
{
    public bool TryPlace(CargoHold minerHold, FleetCargo cargo, Volume volume)
    {
        ArgumentNullException.ThrowIfNull(minerHold);
        ArgumentNullException.ThrowIfNull(cargo);

        if (cargo.FreeSpace < volume)
        {
            return false;
        }

        Volume overflow = minerHold.Fill(volume);

        foreach (CargoHold hold in cargo.Holds.Where(otherHold => otherHold != minerHold))
        {
            overflow = hold.Fill(overflow);
        }

        return true;
    }
}
