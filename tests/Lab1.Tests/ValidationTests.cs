using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class ValidationTests
{
    private readonly MiningCatalog _catalog;

    public ValidationTests()
    {
        _catalog = new MiningCatalog();
    }

    [Fact]
    public void SpeedMustBePositive()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(() => new Speed(0));

        // Assert
        Assert.Equal("astronomicalUnitsPerHour", exception.ParamName);
    }

    [Fact]
    public void TaxRateCannotBeNegative()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(() => new TaxRate(-0.01m));

        // Assert
        Assert.Equal("fraction", exception.ParamName);
    }

    [Fact]
    public void TaxRateCannotExceedOne()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(() => new TaxRate(1.01m));

        // Assert
        Assert.Equal("fraction", exception.ParamName);
    }

    [Fact]
    public void TaxRateAcceptsZeroAndOne()
    {
        // Act
        var zeroRate = new TaxRate(0);
        var fullRate = new TaxRate(1);

        // Assert
        Assert.Equal(0m, zeroRate.Fraction);
        Assert.Equal(1m, fullRate.Fraction);
    }

    [Fact]
    public void OreUnitVolumeMustBePositive()
    {
        // Arrange
        MineralAmount[] portionYield = [new MineralAmount(_catalog.Tritanium, 200)];

        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new Ore("Veldspar", new Volume(0), portionYield));

        // Assert
        Assert.Equal("unitVolume", exception.ParamName);
    }

    [Fact]
    public void OreMustYieldAtLeastOneMineral()
    {
        // Arrange
        MineralAmount[] portionYield = [];

        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new Ore("Veldspar", new Volume(0.1m), portionYield));

        // Assert
        Assert.Equal("portionYield", exception.ParamName);
    }

    [Fact]
    public void ShipHoldMustBePositive()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => _catalog.CreateVenture(new Volume(0)));

        // Assert
        Assert.Equal("holdCapacity", exception.ParamName);
    }

    [Fact]
    public void PriceListCannotContainMineralTwice()
    {
        // Arrange
        MineralPrice[] prices =
        [
            new MineralPrice(_catalog.Tritanium, 4),
            new MineralPrice(_catalog.Tritanium, 5),
        ];

        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(() => new PriceList(prices));

        // Assert
        Assert.Equal("prices", exception.ParamName);
    }

    [Fact]
    public void DeadlineMustBePositive()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new DeadlineCondition(new Duration(0)));

        // Assert
        Assert.Equal("deadline", exception.ParamName);
    }

    [Fact]
    public void TargetVolumeMustBePositive()
    {
        // Act
        ArgumentException exception = Assert.ThrowsAny<ArgumentException>(
            () => new VolumeCondition(new Volume(0)));

        // Assert
        Assert.Equal("targetVolume", exception.ParamName);
    }
}
