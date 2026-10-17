using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public interface IPlacementStrategy
{
    bool TryPlace(CargoHold minerHold, FleetCargo cargo, Volume volume);
}
