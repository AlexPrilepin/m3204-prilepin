using Itmo.ObjectOrientedProgramming.Lab1.Measures;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public readonly record struct ContractProgress
{
    private const decimal CycleHours = 1;

    public ContractProgress(Duration elapsed, Volume minedVolume, Duration flightTime)
    {
        Elapsed = elapsed;
        MinedVolume = minedVolume;
        FlightTime = flightTime;
    }

    public Duration Elapsed { get; }

    public Volume MinedVolume { get; }

    public Duration FlightTime { get; }

    public Duration EarliestTripEnd => Elapsed + FlightTime + new Duration(CycleHours) + FlightTime;

    public Duration ReturnAfterNextCycle => Elapsed + new Duration(CycleHours) + FlightTime;

    public ContractProgress AfterFlight()
    {
        return new ContractProgress(Elapsed + FlightTime, MinedVolume, FlightTime);
    }

    public ContractProgress AfterCycle(Volume cycleVolume)
    {
        return new ContractProgress(Elapsed + new Duration(CycleHours), MinedVolume + cycleVolume, FlightTime);
    }
}
