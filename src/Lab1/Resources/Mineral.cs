namespace Itmo.ObjectOrientedProgramming.Lab1.Resources;

public sealed record Mineral
{
    public Mineral(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
    }

    public string Name { get; }
}
