using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;

namespace Itmo.ObjectOrientedProgramming.Lab1.Calculation;

public sealed class ContractCalculator
{
    private readonly Station _station;

    public ContractCalculator(Station station)
    {
        ArgumentNullException.ThrowIfNull(station);

        _station = station;
    }

    public ContractResult Calculate(Contract contract)
    {
        ArgumentNullException.ThrowIfNull(contract);

        ContractRejection? rejection = contract.FindRejection();

        if (rejection is not null)
        {
            return new ContractResult.Rejected(rejection);
        }

        return new ContractResult.Completed(Execute(contract));
    }

    private ContractReport Execute(Contract contract)
    {
        Ore ore = contract.Belt.Ore;
        ContractProgress progress = contract.CreateInitialProgress();
        var tripVolumes = new List<Volume>();
        var storage = new OreStorage(0);
        int refinedPortions = 0;

        while (contract.AllowsTrip(progress))
        {
            FleetCargo cargo = contract.Fleet.CreateEmptyCargo();
            progress = contract.PerformTrip(cargo, progress);
            tripVolumes.Add(cargo.LoadedVolume);

            storage = storage.Store(ore.CountUnits(cargo.LoadedVolume));
            refinedPortions += storage.FullPortions;
            storage = storage.RemoveFullPortions();
        }

        return CreateReport(contract, progress.Elapsed, tripVolumes, refinedPortions, storage.Units);
    }

    private ContractReport CreateReport(
        Contract contract,
        Duration workingTime,
        IEnumerable<Volume> tripVolumes,
        int refinedPortions,
        int remainingUnits)
    {
        IReadOnlyCollection<MineralAmount> minerals = contract.Belt.Ore.CalculateYield(refinedPortions);
        decimal revenue = contract.PriceList.CalculateRevenue(minerals);
        decimal rent = contract.Fleet.CalculateRent(workingTime);
        decimal tax = _station.CalculateTax(revenue);

        return new ContractReport(workingTime, tripVolumes, minerals, remainingUnits, revenue, rent, tax);
    }
}
