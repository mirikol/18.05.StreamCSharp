using Spectre.Console;
using System.Runtime.InteropServices;

internal class Program
{
    private static async Task Main()
    {
        try
        {
            WindowSettings.Initialize();
            Logger.enabled = true;

            //var goldenSword = new Sword("Golden sword", 0, 10, 200);
            //var commonSword = new Sword("Common sword", 0, 5, 100);

            //var glove = new Glove("Glove", 10, 0);
            //var greave = new Greave("Greave", 20, 5);
            //var helmet = new Helmet("Helmet", 0, 10);
            //var vest = new Vest("Vest", 100, 0);

            //SaveLoad<IWeapon>.Save(goldenSword, goldenSword.Name);
            //SaveLoad<IWeapon>.Save(commonSword, commonSword.Name);

            //SaveLoad<IArmor>.Save(glove, glove.Name);
            //SaveLoad<IArmor>.Save(greave, greave.Name);
            //SaveLoad<IArmor>.Save(helmet, helmet.Name);
            //SaveLoad<IArmor>.Save(vest, vest.Name);

            //var unit1 = new Unit(SaveLoad<UnitModel>.Load("Timur"));
            //EquipUtility.EquipUnit(unit1, goldenSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit1, vest, BodyPartName.Body);
            //EquipUtility.EquipUnit(unit1, helmet, BodyPartName.Head);
            //EquipUtility.EquipUnit(unit1, glove, BodyPartName.RightArm);
            //var unitSave1 = new UnitSave(unit1);
            //SaveLoad<UnitSave>.Save(unitSave1, "Player");

            //var unit2 = new Unit(SaveLoad<UnitModel>.Load("Gregory"));
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.LeftArm);
            //var unitSave2 = new UnitSave(unit2);
            //SaveLoad<UnitSave>.Save(unitSave2, "Gregory");

            //var unit3 = new Unit(SaveLoad<UnitModel>.Load("Michael"));
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.LeftArm);
            //var unitSave3 = new UnitSave(unit3);
            //SaveLoad<UnitSave>.Save(unitSave3, "Michael");

            //var playerUnits = new List<Unit>();
            //var enemyUnits = new List<Unit>();
            //playerUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Player")));
            //enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Gregory")));
            //enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Michael")));

            //ArenaModel model = new ArenaModel(playerUnits.Select(x => x.Model.Name).ToList(), enemyUnits.Select(x => x.Model.Name).ToList());
            //SaveLoad<ArenaModel>.Save(model, "Title");

            //Arena arena = new Arena(SaveLoad<ArenaModel>.Load("Title"));
            //arena.Start();

            IConsoleRenderer renderer = new BufferedConsoleRenderer();
            renderer.SetSize(56, 18);
            DrawUtils draw = new DrawUtils(renderer.Buffer);
            draw.ResetColor();

            int frame = 0;

            while (true)
            {
                frame++;

                renderer.Buffer.Set(0 + frame % 15, 0, 'B', 15, 0);
                renderer.Buffer.Set(1 + frame % 15, 0, 'o', 15, 0);
                renderer.Buffer.Set(2 + frame % 15, 0, 'o', 15, 0);
                renderer.Buffer.Set(3 + frame % 15, 0, 'b', 15, 0);
                renderer.Buffer.Set(4 + frame % 15, 0, 's', 15, 0);

                renderer.Render();
                await Task.Delay(500);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }

    //private const int NumberOfRows = 10;

    //private static readonly Random _random = new();
    //private static readonly string[] _exchanges = new string[]
    //{
    //        "SGD", "SEK", "PLN",
    //        "MYR", "EUR", "USD",
    //        "AUD", "JPY", "CNH",
    //        "HKD", "CAD", "INR",
    //        "DKK", "GBP", "RUB",
    //        "NZD", "MXN", "IDR",
    //        "TWD", "THB", "VND",
    //};

    //public static async Task Main(string[] args)
    //{
    //    DisableScrolling();

    //    // Write a markup line to the console
    //    AnsiConsole.MarkupLine("[yellow]Hello[/], [blue]World[/]!");

    //    // Write text to the console
    //    AnsiConsole.WriteLine("Hello, World!");

    //    // Write a table to the console
    //    AnsiConsole.Write(new Table()
    //        .RoundedBorder()
    //        .AddColumns("[red]Greeting[/]", "[red]Subject[/]")
    //        .AddRow("[yellow]Hello[/]", "World")
    //        .AddRow("[green]Oh hi[/]", "[blue u]Mark[/]"));
    //}

    //private static void AddExchangeRateRow(Table table)
    //{
    //    var (source, destination, rate) = GetExchangeRate();
    //    table.AddRow(
    //        source, destination,
    //        _random.NextDouble() > 0.35D ? $"[green]{rate}[/]" : $"[red]{rate}[/]");
    //}

    //private static (string Source, string Destination, double Rate) GetExchangeRate()
    //{
    //    var source = _exchanges[_random.Next(0, _exchanges.Length)];
    //    var dest = _exchanges[_random.Next(0, _exchanges.Length)];
    //    var rate = 200 / ((_random.NextDouble() * 320) + 1);

    //    while (source == dest)
    //    {
    //        dest = _exchanges[_random.Next(0, _exchanges.Length)];
    //    }

    //    return (source, dest, rate);
    //}

}