using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class VolumeCondition : IContractCondition
{
    private readonly Volume _targetVolume;

    public VolumeCondition(Volume targetVolume)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(targetVolume.CubicMeters, nameof(targetVolume));

        _targetVolume = targetVolume;
    }

    public ContractRejection? FindRejection(ContractProgress initialProgress)
    {
        return null;
    }

    public bool AllowsTrip(ContractProgress progress)
    {
        return IsBelowTarget(progress);
    }

    public bool AllowsCycle(ContractProgress progress)
    {
        return IsBelowTarget(progress);
    }

    private bool IsBelowTarget(ContractProgress progress)
    {
        return progress.MinedVolume < _targetVolume;
    }
}
