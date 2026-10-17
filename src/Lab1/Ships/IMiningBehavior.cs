using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public interface IMiningBehavior
{
    Volume CalculateCycleYield(int completedCycles);
}
