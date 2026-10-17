using Itmo.ObjectOrientedProgramming.Lab1.Calculation;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class RefiningTests
{
    private readonly MiningCatalog _catalog;
    private readonly ContractCalculator _calculator;

    public RefiningTests()
    {
        _catalog = new MiningCatalog();
        _calculator = new ContractCalculator(new Station(new TaxRate(0)));
    }

    [Fact]
    public void IncompletePortionStaysInStationStorage()
    {
        // Arrange
        Contract contract = _catalog.CreateContract(
            _catalog.Pyroxeres,
            _catalog.CreateDefaultFleet(),
            new VolumeCondition(new Volume(100)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(new Duration(3), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Pyerite, 135), report.Minerals);
        Assert.Contains(new MineralAmount(_catalog.Mexallon, 45), report.Minerals);
        Assert.Equal(33, report.RemainingOreUnits);
        Assert.Equal(4500m, report.Revenue);
        Assert.Equal(3000m, report.Rent);
        Assert.Equal(1500m, report.Profit);
    }

    [Fact]
    public void StoredUnitsAreRefinedTogetherWithNextTrip()
    {
        // Arrange
        Contract contract = _catalog.CreateContract(
            _catalog.Pyroxeres,
            _catalog.CreateDefaultFleet(),
            new DeadlineCondition(new Duration(10)));
        Volume[] expectedTripVolumes = [new Volume(400), new Volume(200)];

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(expectedTripVolumes, report.TripVolumes);
        Assert.Contains(new MineralAmount(_catalog.Pyerite, 855), report.Minerals);
        Assert.Contains(new MineralAmount(_catalog.Mexallon, 285), report.Minerals);
        Assert.Equal(99, report.RemainingOreUnits);
        Assert.Equal(28500m, report.Revenue);
        Assert.Equal(10000m, report.Rent);
        Assert.Equal(18500m, report.Profit);
    }
}
