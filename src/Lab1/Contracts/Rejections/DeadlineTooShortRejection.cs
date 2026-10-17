using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;

public sealed record DeadlineTooShortRejection(Duration Deadline, Duration RequiredTime) : ContractRejection;
