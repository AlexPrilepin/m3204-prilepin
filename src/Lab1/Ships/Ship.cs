using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class Ship
{
    private readonly IMiningBehavior _miningBehavior;

    public Ship(string name, Volume holdCapacity, Speed speed, decimal rentPerHour, IMiningBehavior miningBehavior)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(holdCapacity.CubicMeters, nameof(holdCapacity));
        ArgumentNullException.ThrowIfNull(speed);
        ArgumentOutOfRangeException.ThrowIfNegative(rentPerHour);
        ArgumentNullException.ThrowIfNull(miningBehavior);

        Name = name;
        HoldCapacity = holdCapacity;
        Speed = speed;
        RentPerHour = rentPerHour;
        _miningBehavior = miningBehavior;
    }

    public string Name { get; }

    public Volume HoldCapacity { get; }

    public Speed Speed { get; }

    public decimal RentPerHour { get; }

    public Volume CalculateCycleYield(int completedCycles)
    {
        return _miningBehavior.CalculateCycleYield(completedCycles);
    }
}
