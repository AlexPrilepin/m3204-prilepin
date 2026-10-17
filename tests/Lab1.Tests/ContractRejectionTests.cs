using Itmo.ObjectOrientedProgramming.Lab1.Calculation;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Itmo.ObjectOrientedProgramming.Lab1.Stations;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class ContractRejectionTests
{
    private readonly MiningCatalog _catalog;
    private readonly ContractCalculator _calculator;

    public ContractRejectionTests()
    {
        _catalog = new MiningCatalog();
        _calculator = new ContractCalculator(new Station(new TaxRate(0)));
    }

    [Fact]
    public void FleetOfCarriersIsRejected()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Epithal], new SharedHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        Assert.IsType<FleetCannotMineRejection>(rejection);
    }

    [Fact]
    public void MinerWithTooSmallSeparateHoldIsRejected()
    {
        // Arrange
        Ship smallVenture = _catalog.CreateVenture(new Volume(50));
        var fleet = new Fleet([smallVenture], new SeparateHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(10)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        Assert.IsType<FleetCannotMineRejection>(rejection);
    }

    [Fact]
    public void MissingPriceIsCheckedBeforeFleetCannotMine()
    {
        // Arrange
        var contract = new Contract(
            _catalog.CreateBelt(_catalog.Scordite),
            new Fleet([_catalog.Epithal], new SharedHoldPlacement()),
            new PriceList([new MineralPrice(_catalog.Tritanium, 4)]),
            new DeadlineCondition(new Duration(6)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        Assert.Equal(new MissingPriceRejection(_catalog.Pyerite), rejection);
    }

    [Fact]
    public void FleetCannotMineIsCheckedBeforeDeadline()
    {
        // Arrange
        var fleet = new Fleet([_catalog.Epithal], new SharedHoldPlacement());
        Contract contract = _catalog.CreateContract(_catalog.Veldspar, fleet, new DeadlineCondition(new Duration(1)));

        // Act
        ContractResult result = _calculator.Calculate(contract);

        // Assert
        ContractRejection rejection = Assert.IsType<ContractResult.Rejected>(result).Rejection;
        Assert.IsType<FleetCannotMineRejection>(rejection);
    }
}
