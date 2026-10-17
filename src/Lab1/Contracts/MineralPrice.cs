using Itmo.ObjectOrientedProgramming.Lab1.Resources;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed record MineralPrice
{
    public MineralPrice(Mineral mineral, decimal pricePerUnit)
    {
        ArgumentNullException.ThrowIfNull(mineral);
        ArgumentOutOfRangeException.ThrowIfNegative(pricePerUnit);

        Mineral = mineral;
        PricePerUnit = pricePerUnit;
    }

    public Mineral Mineral { get; }

    public decimal PricePerUnit { get; }

    public decimal CalculateCost(int quantity)
    {
        return PricePerUnit * quantity;
    }
}
