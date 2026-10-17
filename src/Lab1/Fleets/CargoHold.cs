using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Fleets;

public sealed class CargoHold
{
    public CargoHold(Ship ship)
    {
        ArgumentNullException.ThrowIfNull(ship);

        Ship = ship;
        LoadedVolume = new Volume(0);
    }

    public Ship Ship { get; }

    public Volume LoadedVolume { get; private set; }

    public Volume FreeSpace => Ship.HoldCapacity - LoadedVolume;

    public Volume Fill(Volume volume)
    {
        Volume freeSpace = FreeSpace;

        if (freeSpace < volume)
        {
            LoadedVolume = Ship.HoldCapacity;

            return volume - freeSpace;
        }

        LoadedVolume += volume;

        return new Volume(0);
    }
}
