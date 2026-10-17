using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Resources;

public sealed class AsteroidBelt
{
    public AsteroidBelt(string name, Distance distance, Ore ore)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(ore);

        Name = name;
        Distance = distance;
        Ore = ore;
    }

    public string Name { get; }

    public Distance Distance { get; }

    public Ore Ore { get; }
}
