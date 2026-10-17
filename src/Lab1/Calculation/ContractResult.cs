using Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;

namespace Itmo.ObjectOrientedProgramming.Lab1.Calculation;

public abstract record ContractResult
{
    private ContractResult()
    {
    }

    public sealed record Completed(ContractReport Report) : ContractResult;

    public sealed record Rejected(ContractRejection Rejection) : ContractResult;
}
