using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Stations;

public sealed class Station
{
    private readonly TaxRate _taxRate;

    public Station(TaxRate taxRate)
    {
        _taxRate = taxRate;
    }

    public decimal CalculateTax(decimal revenue)
    {
        return _taxRate.ApplyTo(revenue);
    }
}
