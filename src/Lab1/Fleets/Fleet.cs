using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public sealed class Fleet
{
    private readonly IReadOnlyCollection<Ship> _ships;
    private readonly IPlacementStrategy _placementStrategy;

    public Fleet(IReadOnlyCollection<Ship> ships, IPlacementStrategy placementStrategy)
    {
        ArgumentNullException.ThrowIfNull(ships);
        ArgumentNullException.ThrowIfNull(placementStrategy);

        if (ships.Count == 0)
        {
            throw new ArgumentException("Fleet must contain at least one ship.", nameof(ships));
        }

        _ships = ships.ToArray();
        _placementStrategy = placementStrategy;
        Speed = new Speed(_ships.Min(ship => ship.Speed.AstronomicalUnitsPerHour));
    }

    public Speed Speed { get; }

    public Duration CalculateFlightTime(Distance distance)
    {
        return distance / Speed;
    }

    public decimal CalculateRent(Duration workingTime)
    {
        return _ships.Sum(ship => ship.RentPerHour) * workingTime.Hours;
    }

    public FleetCargo CreateEmptyCargo()
    {
        return new FleetCargo(_ships.Select(ship => new CargoHold(ship)));
    }

    public Volume MineCycle(FleetCargo cargo, int completedCycles)
    {
        ArgumentNullException.ThrowIfNull(cargo);

        var minedVolume = new Volume(0);

        foreach (CargoHold hold in cargo.Holds)
        {
            Volume shipYield = hold.Ship.CalculateCycleYield(completedCycles);

            if (_placementStrategy.TryPlace(hold, cargo, shipYield))
            {
                minedVolume += shipYield;
            }
        }

        return minedVolume;
    }

    public bool CanMineFromEmptyHolds()
    {
        Volume firstCycleVolume = MineCycle(CreateEmptyCargo(), completedCycles: 0);

        return !firstCycleVolume.IsZero;
    }
}
