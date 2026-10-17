using Itmo.ObjectOrientedProgramming.Lab1.Resources;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts.Rejections;

public sealed record MissingPriceRejection(Mineral Mineral) : ContractRejection;
