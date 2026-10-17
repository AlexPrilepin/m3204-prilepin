using Itmo.ObjectOrientedProgramming.Lab1.Calculation;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class FleetTests
{
    private readonly MiningCatalog _catalog;
    private readonly ContractCalculator _calculator;

    public FleetTests()
    {
        _catalog = new MiningCatalog();
        _calculator = new ContractCalculator(new Station(new TaxRate(0)));
    }

    [Fact]
    public void SharedHoldPlacesOverflowIntoNextShip()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Venture, _catalog.Epithal], new SharedHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Volume tripVolume = Assert.Single(report.TripVolumes);
        Assert.Equal(new Volume(600), tripVolume);
        Assert.Equal(new Duration(10), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 12000), report.Minerals);
        Assert.Equal(48000m, report.Revenue);
        Assert.Equal(15000m, report.Rent);
        Assert.Equal(33000m, report.Profit);
    }

    [Fact]
    public void SeparateHoldsKeepOreInMinerHoldOnly()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Venture, _catalog.Epithal], new SeparateHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Volume tripVolume = Assert.Single(report.TripVolumes);
        Assert.Equal(new Volume(400), tripVolume);
        Assert.Equal(new Duration(8), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 8000), report.Minerals);
        Assert.Equal(32000m, report.Revenue);
        Assert.Equal(12000m, report.Rent);
        Assert.Equal(20000m, report.Profit);
    }

    [Fact]
    public void ShipIdlesWhenSharedHoldsCannotFitItsWholeYield()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Venture, _catalog.Prospect], new SharedHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Volume tripVolume = Assert.Single(report.TripVolumes);
        Assert.Equal(new Volume(1375), tripVolume);
        Assert.Equal(new Duration(9), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 27400), report.Minerals);
        Assert.Equal(50, report.RemainingOreUnits);
        Assert.Equal(109600m, report.Revenue);
        Assert.Equal(22500m, report.Rent);
        Assert.Equal(87100m, report.Profit);
    }

    [Fact]
    public void SharedHoldPutsOverflowIntoOtherHoldsInFleetOrder()
    {
        // Arrange
        var minerHold = new CargoHold(_catalog.CreateVenture(new Volume(50)));
        var cargo = new FleetCargo([new CargoHold(_catalog.Venture), minerHold, new CargoHold(_catalog.Epithal)]);
        var placement = new SharedHoldPlacement();

        // Act
        bool isPlaced = placement.TryPlace(minerHold, cargo, new Volume(100));

        // Assert
        Assert.True(isPlaced);
        Assert.Collection(
            cargo.Holds,
            hold => Assert.Equal(new Volume(50), hold.LoadedVolume),
            hold => Assert.Equal(new Volume(50), hold.LoadedVolume),
            hold => Assert.Equal(new Volume(0), hold.LoadedVolume));
    }

    [Fact]
    public void ProspectStartsEveryTripWithWarmUpCycle()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Prospect], new SharedHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new VolumeCondition(new Volume(1200)));
        Volume[] expectedTripVolumes = [new Volume(975), new Volume(225)];

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(expectedTripVolumes, report.TripVolumes);
        Assert.Equal(new Duration(12.2m), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 24000), report.Minerals);
        Assert.Equal(0, report.RemainingOreUnits);
        Assert.Equal(96000m, report.Revenue);
        Assert.Equal(18300m, report.Rent);
        Assert.Equal(77700m, report.Profit);
    }
}
