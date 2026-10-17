namespace Itmo.ObjectOrientedProgramming.Lab1.Resources;

public sealed record MineralAmount
{
    public MineralAmount(Mineral mineral, int quantity)
    {
        ArgumentNullException.ThrowIfNull(mineral);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);

        Mineral = mineral;
        Quantity = quantity;
    }

    public Mineral Mineral { get; }

    public int Quantity { get; }

    public MineralAmount MultiplyBy(int factor)
    {
        return new MineralAmount(Mineral, Quantity * factor);
    }
}
