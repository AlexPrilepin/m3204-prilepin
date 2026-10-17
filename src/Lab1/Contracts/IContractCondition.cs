using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public interface IContractCondition
{
    ContractRejection? FindRejection(ContractProgress initialProgress);

    bool AllowsTrip(ContractProgress progress);

    bool AllowsCycle(ContractProgress progress);
}
