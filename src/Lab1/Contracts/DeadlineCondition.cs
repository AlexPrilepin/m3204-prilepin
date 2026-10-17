using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class DeadlineCondition : IContractCondition
{
    private readonly Duration _deadline;

    public DeadlineCondition(Duration deadline)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deadline.Hours, nameof(deadline));

        _deadline = deadline;
    }

    public ContractRejection? FindRejection(ContractProgress initialProgress)
    {
        Duration requiredTime = initialProgress.EarliestTripEnd;

        if (requiredTime > _deadline)
        {
            return new DeadlineTooShortRejection(_deadline, requiredTime);
        }

        return null;
    }

    public bool AllowsTrip(ContractProgress progress)
    {
        return progress.EarliestTripEnd < _deadline;
    }

    public bool AllowsCycle(ContractProgress progress)
    {
        return progress.ReturnAfterNextCycle <= _deadline;
    }
}
