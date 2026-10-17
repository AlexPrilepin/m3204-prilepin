using Itmo.ObjectOrientedProgramming.Lab1.Resources;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class PriceList
{
    private readonly IReadOnlyCollection<MineralPrice> _prices;

    public PriceList(IReadOnlyCollection<MineralPrice> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);

        int pricedMineralsCount = prices
            .DistinctBy(price => price.Mineral)
            .Count();

        if (pricedMineralsCount != prices.Count)
        {
            throw new ArgumentException("Each mineral must have only one price.", nameof(prices));
        }

        _prices = prices.ToArray();
    }

    public Mineral? FindUnpricedMineral(IEnumerable<Mineral> minerals)
    {
        ArgumentNullException.ThrowIfNull(minerals);

        return minerals.FirstOrDefault(mineral => !HasPrice(mineral));
    }

    public decimal CalculateRevenue(IEnumerable<MineralAmount> minerals)
    {
        ArgumentNullException.ThrowIfNull(minerals);

        return minerals
            .Join(
                _prices,
                amount => amount.Mineral,
                price => price.Mineral,
                (amount, price) => price.CalculateCost(amount.Quantity))
            .Sum();
    }

    private bool HasPrice(Mineral mineral)
    {
        return _prices.Any(price => price.Mineral.Equals(mineral));
    }
}
