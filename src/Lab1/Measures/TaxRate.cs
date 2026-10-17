namespace Itmo.ObjectOrientedProgramming.Lab1.Measures;

public readonly record struct TaxRate
{
    private const decimal MaximumFraction = 1;

    public TaxRate(decimal fraction)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(fraction);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(fraction, MaximumFraction);

        Fraction = fraction;
    }

    public decimal Fraction { get; }

    public decimal ApplyTo(decimal revenue)
    {
        return revenue * Fraction;
    }
}
