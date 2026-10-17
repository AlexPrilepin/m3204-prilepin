using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class ConstantMining : IMiningBehavior
{
    private readonly Volume _cycleYield;

    public ConstantMining(Volume cycleYield)
    {
        _cycleYield = cycleYield;
    }

    public Volume CalculateCycleYield(int completedCycles)
    {
        return _cycleYield;
    }
}
