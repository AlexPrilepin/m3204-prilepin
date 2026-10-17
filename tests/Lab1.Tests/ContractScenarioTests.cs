using Itmo.ObjectOrientedProgramming.Lab1.Calculation;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class ContractScenarioTests
{
    private readonly MiningCatalog _catalog;
    private readonly ContractCalculator _calculator;

    public ContractScenarioTests()
    {
        _catalog = new MiningCatalog();
        _calculator = new ContractCalculator(new Station(new TaxRate(0)));
    }

    [Fact]
    public void TimeContractWithSingleVentureIsCompleted()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(6)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(1, report.TripCount);
        Assert.Equal(new Duration(6), report.WorkingTime);
        Assert.Equal(new Volume(400), report.MinedVolume);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 8000), report.Minerals);
        Assert.Equal(0, report.RemainingOreUnits);
        Assert.Equal(32000m, report.Revenue);
        Assert.Equal(6000m, report.Rent);
        Assert.Equal(26000m, report.Profit);
    }

    [Fact]
    public void FleetWithoutShipsCannotBeCreated()
    {
        // Arrange
        List<Ship> ships = [];
        var placementStrategy = new SharedHoldPlacement();

        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(() => new Fleet(ships, placementStrategy));

        // Assert
        Assert.Equal("ships", exception.ParamName);
    }

    [Fact]
    public void VolumeContractWithSingleVentureIsCompleted()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new VolumeCondition(new Volume(1000)));
        Volume[] expectedTripVolumes = [new Volume(400), new Volume(400), new Volume(200)];

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(3, report.TripCount);
        Assert.Equal(expectedTripVolumes, report.TripVolumes);
        Assert.Equal(new Volume(1000), report.MinedVolume);
        Assert.Equal(new Duration(16), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 20000), report.Minerals);
        Assert.Equal(80000m, report.Revenue);
        Assert.Equal(16000m, report.Rent);
        Assert.Equal(64000m, report.Profit);
    }

    [Fact]
    public void TooShortTimeContractIsRejected()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(2)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        DeadlineTooShortRejection deadlineRejection = Assert.IsType<DeadlineTooShortRejection>(rejection);
        Assert.Equal(new Duration(2), deadlineRejection.Deadline);
        Assert.Equal(new Duration(3), deadlineRejection.RequiredTime);
    }

    [Fact]
    public void ContractWithIncompletePriceListIsRejected()
    {
        // Arrange
        MineralPrice[] prices =
        [
            new MineralPrice(_catalog.Tritanium, 4),
            new MineralPrice(_catalog.Mexallon, 70),
            new MineralPrice(_catalog.Isogen, 150),
        ];

        var contract = new Contract(
            _catalog.CreateBelt(_catalog.Scordite),
            _catalog.CreateDefaultFleet(),
            new PriceList(prices),
            new DeadlineCondition(new Duration(6)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        Assert.Equal(new MissingPriceRejection(_catalog.Pyerite), rejection);
    }
}
