using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class NonMining : IMiningBehavior
{
    public Volume CalculateCycleYield(int completedCycles)
    {
        return new Volume(0);
    }
}
