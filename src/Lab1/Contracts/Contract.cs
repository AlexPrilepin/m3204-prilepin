using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class Contract
{
    private readonly IContractCondition _condition;

    public Contract(AsteroidBelt belt, Fleet fleet, PriceList priceList, IContractCondition condition)
    {
        ArgumentNullException.ThrowIfNull(belt);
        ArgumentNullException.ThrowIfNull(fleet);
        ArgumentNullException.ThrowIfNull(priceList);
        ArgumentNullException.ThrowIfNull(condition);

        Belt = belt;
        Fleet = fleet;
        PriceList = priceList;
        _condition = condition;
    }

    public AsteroidBelt Belt { get; }

    public Fleet Fleet { get; }

    public PriceList PriceList { get; }

    public ContractRejection? FindRejection()
    {
        Mineral? unpricedMineral = PriceList.FindUnpricedMineral(Belt.Ore.Minerals);

        if (unpricedMineral is not null)
        {
            return new MissingPriceRejection(unpricedMineral);
        }

        if (!Fleet.CanMineFromEmptyHolds())
        {
            return new FleetCannotMineRejection();
        }

        return _condition.FindRejection(CreateInitialProgress());
    }

    public ContractProgress CreateInitialProgress()
    {
        Duration flightTime = Fleet.CalculateFlightTime(Belt.Distance);

        return new ContractProgress(new Duration(0), new Volume(0), flightTime);
    }

    public bool AllowsTrip(ContractProgress progress)
    {
        return _condition.AllowsTrip(progress);
    }

    public ContractProgress PerformTrip(FleetCargo cargo, ContractProgress tripStart)
    {
        ArgumentNullException.ThrowIfNull(cargo);

        ContractProgress progress = tripStart.AfterFlight();
        int completedCycles = 0;

        while (_condition.AllowsCycle(progress))
        {
            Volume cycleVolume = Fleet.MineCycle(cargo, completedCycles);

            if (cycleVolume.IsZero)
            {
                break;
            }

            progress = progress.AfterCycle(cycleVolume);
            completedCycles++;
        }

        return progress.AfterFlight();
    }
}
