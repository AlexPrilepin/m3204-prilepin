using Itmo.ObjectOrientedProgramming.Lab1.Calculation;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class DeadlineContractTests
{
    private readonly MiningCatalog _catalog;
    private readonly ContractCalculator _calculator;

    public DeadlineContractTests()
    {
        _catalog = new MiningCatalog();
        _calculator = new ContractCalculator(new Station(new TaxRate(0)));
    }

    [Fact]
    public void DeadlineEqualToShortestTripIsAcceptedButStartsNoTrips()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(3)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Empty(report.TripVolumes);
        Assert.Equal(new Duration(0), report.WorkingTime);
        Assert.Equal(new Volume(0), report.MinedVolume);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 0), report.Minerals);
        Assert.Equal(0m, report.Revenue);
        Assert.Equal(0m, report.Rent);
        Assert.Equal(0m, report.Profit);
    }

    [Fact]
    public void LastTripMinesOnlyCyclesThatFitBeforeDeadline()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(10)));
        Volume[] expectedTripVolumes = [new Volume(400), new Volume(200)];

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(expectedTripVolumes, report.TripVolumes);
        Assert.Equal(new Duration(10), report.WorkingTime);
        Assert.Contains(new MineralAmount(_catalog.Tritanium, 12000), report.Minerals);
        Assert.Equal(48000m, report.Revenue);
        Assert.Equal(10000m, report.Rent);
        Assert.Equal(38000m, report.Profit);
    }

    [Fact]
    public void StationTakesTaxFromRevenue()
    {
        // Arrange
        var calculator = new ContractCalculator(new Station(new TaxRate(0.05m)));
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(6)));

        // Act
        ContractResult result = calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(32000m, report.Revenue);
        Assert.Equal(1600m, report.Tax);
        Assert.Equal(24400m, report.Profit);
    }

    [Fact]
    public void ContractWithNegativeProfitIsCompleted()
    {
        // Arrange
        var contract = new Contract(
            _catalog.CreateBelt(_catalog.Veldspar),
            _catalog.CreateDefaultFleet(),
            new PriceList([new MineralPrice(_catalog.Tritanium, 0.5m)]),
            new DeadlineCondition(new Duration(6)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractReport report = Assert.IsType<ContractResult.Completed>(result).Report;
        Assert.Equal(4000m, report.Revenue);
        Assert.Equal(6000m, report.Rent);
        Assert.Equal(-2000m, report.Profit);
    }

    [Fact]
    public void RepeatedCalculationOfSameContractGivesSameReport()
    {
        // Arrange
        Contract contract = _catalog.CreateDefaultContract(new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult firstResult = _calculator.Calculate(contract);
        ContractResult secondResult = _calculator.Calculate(contract);

        // Assert
        ContractReport firstReport = Assert.IsType<ContractResult.Completed>(firstResult).Report;
        ContractReport secondReport = Assert.IsType<ContractResult.Completed>(secondResult).Report;
        Assert.Equal(firstReport.TripVolumes, secondReport.TripVolumes);
        Assert.Equal(firstReport.WorkingTime, secondReport.WorkingTime);
        Assert.Equal(firstReport.Minerals, secondReport.Minerals);
        Assert.Equal(firstReport.Profit, secondReport.Profit);
    }
}
