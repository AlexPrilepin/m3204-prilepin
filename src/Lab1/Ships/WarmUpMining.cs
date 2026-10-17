using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class WarmUpMining : IMiningBehavior
{
    private readonly Volume _firstCycleYield;
    private readonly Volume _regularCycleYield;

    public WarmUpMining(Volume firstCycleYield, Volume regularCycleYield)
    {
        _firstCycleYield = firstCycleYield;
        _regularCycleYield = regularCycleYield;
    }

    public Volume CalculateCycleYield(int completedCycles)
    {
        if (completedCycles == 0)
        {
            return _firstCycleYield;
        }

        return _regularCycleYield;
    }
}
