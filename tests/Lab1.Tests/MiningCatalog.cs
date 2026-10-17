using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Fleets;
using Itmo.ObjectOrientedProgramming.Lab1.Measures;
using Itmo.ObjectOrientedProgramming.Lab1.Resources;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public sealed class MiningCatalog
{
    private readonly IMiningBehavior _ventureMining;
    private readonly Distance _beltDistance;

    public MiningCatalog()
    {
        _ventureMining = new ConstantMining(new Volume(100));
        _beltDistance = new Distance(4);

        Tritanium = new Mineral("Tritanium");
        Pyerite = new Mineral("Pyerite");
        Mexallon = new Mineral("Mexallon");
        Isogen = new Mineral("Isogen");

        Veldspar = new Ore("Veldspar", new Volume(0.1m), [new MineralAmount(Tritanium, 200)]);

        Scordite = new Ore(
            "Scordite",
            new Volume(0.15m),
            [new MineralAmount(Tritanium, 75), new MineralAmount(Pyerite, 55)]);

        Pyroxeres = new Ore(
            "Pyroxeres",
            new Volume(0.3m),
            [new MineralAmount(Pyerite, 45), new MineralAmount(Mexallon, 15)]);

        Kernite = new Ore(
            "Kernite",
            new Volume(1.2m),
            [new MineralAmount(Mexallon, 30), new MineralAmount(Isogen, 60)]);

        Venture = CreateVenture(new Volume(400));

        Prospect = new Ship(
            "Prospect",
            new Volume(1000),
            new Speed(5),
            1500,
            new WarmUpMining(new Volume(75), new Volume(150)));

        Epithal = new Ship("Epithal", new Volume(3000), new Speed(2), 500, new NonMining());
    }

    public Mineral Tritanium { get; }

    public Mineral Pyerite { get; }

    public Mineral Mexallon { get; }

    public Mineral Isogen { get; }

    public Ore Veldspar { get; }

    public Ore Scordite { get; }

    public Ore Pyroxeres { get; }

    public Ore Kernite { get; }

    public Ship Venture { get; }

    public Ship Prospect { get; }

    public Ship Epithal { get; }

    public Ship CreateVenture(Volume holdCapacity)
    {
        return new Ship("Venture", holdCapacity, new Speed(4), 1000, _ventureMining);
    }

    public Fleet CreateDefaultFleet()
    {
        return new Fleet([Venture], new SharedHoldPlacement());
    }

    public PriceList CreateDefaultPriceList()
    {
        MineralPrice[] prices =
        [
            new MineralPrice(Tritanium, 4),
            new MineralPrice(Pyerite, 10),
            new MineralPrice(Mexallon, 70),
            new MineralPrice(Isogen, 150),
        ];

        return new PriceList(prices);
    }

    public AsteroidBelt CreateBelt(Ore ore)
    {
        return new AsteroidBelt("Belt A", _beltDistance, ore);
    }

    public Contract CreateContract(Ore ore, Fleet fleet, IContractCondition condition)
    {
        return new Contract(CreateBelt(ore), fleet, CreateDefaultPriceList(), condition);
    }

    public Contract CreateDefaultContract(IContractCondition condition)
    {
        return CreateContract(Veldspar, CreateDefaultFleet(), condition);
    }
}
