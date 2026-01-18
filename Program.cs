using ANSIConsole;
using Spectre.Console;

public class GameRender
{
    private Layout _layout;

    private Table _outputTable;
    private Panel _outputPanel;
    private int rows = 0;

    public GameRender()
    {
        InitializeLayout();
        StartRender();
    }

    private void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Battle").Ratio(4)
                   .SplitRows(
                       new Layout("Turn").Ratio(2)
                           .SplitColumns(
                               new Layout("Units").Ratio(1),
                               new Layout("Skill").Ratio(1),
                               new Layout("Misc").Ratio(2)
                           ),
                       new Layout("Output").Ratio(2)
                   ),
               new Layout("Info").Ratio(2)
                   .SplitRows(
                       new Layout("Equipment").Ratio(3),
                       new Layout("Stats").Ratio(1)
                   )
           );
        _layout["Battle"]["Turn"]["Misc"].Update(new Panel("").Expand().BorderColor(Color.Black));
        _outputTable = new Table().AddColumn("Text").AddColumn("Time").Border(TableBorder.None);
        _outputPanel = new Panel(_outputTable).Header("Printer").BorderColor(Color.White).Expand();
        _layout["Battle"]["Output"].Update(_outputTable);
    }

    private void StartRender()
    {
        AnsiConsole.Live(_layout)
            .Start(ctx =>
            {
                ctx.Refresh();
                Printer.HasPrinted += (PrinterContext context) =>
                {
                    rows++;
                    if (rows > 22)
                    {
                        _outputTable.RemoveRow(0);
                    }

                    _outputTable.AddRow(context.Text, DateTime.Now.ToString());

                    _layout["Battle"]["Output"].Update(_outputPanel);
                    ctx.Refresh();
                };
                while (true) Thread.Sleep(1000);
            });
    }
}

internal class Program
{
    public static Action<string> WindowHasChanged;

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

            //IConsoleRenderer renderer = new BufferedConsoleRenderer();
            //renderer.SetSize(160, 100);
            //DrawUtils draw = new DrawUtils(renderer.Buffer);
            //draw.ResetColor();

            int frame = 0;

            Task.Run(() =>
            {
                while (true)
                {
                    frame++;
                    WindowHasChanged?.Invoke(frame.ToString());
                    Thread.Sleep(100);
                    Printer.Print("Hello", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("He235235llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("Hel3426346lo", ConsoleColor.White);
                    Printer.Print("Hel4578548lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);
                    Printer.Print("He235235llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("Hel3426346lo", ConsoleColor.White);
                    Printer.Print("Hel4578548lo", ConsoleColor.White);
                    Printer.Print("He235235llo", ConsoleColor.White);
                    Printer.Print("He7457457llo", ConsoleColor.White);
                    Printer.Print("Hel3426346lo", ConsoleColor.White);
                    Printer.Print("Hel4578548lo", ConsoleColor.White);
                    Printer.Print("Hel346346lo", ConsoleColor.White);

                }
            });

            GameRender render = new GameRender();

            //var table = new Table().RoundedBorder();

            //table.AddColumn("ID");
            //table.AddColumn("Status");
            //table.AddColumn("Progress");

            //// Add initial rows
            //table.AddRow("Task 1", "[yellow]Pending[/]", "0%");
            //table.AddRow("Task 2", "[yellow]Pending[/]", "0%");
            //table.AddRow("Task 3", "[yellow]Pending[/]", "0%");

            //// Update cells dynamically
            //table.UpdateCell(0, 1, new Markup("[green]Complete[/]"));
            //table.UpdateCell(0, 2, new Markup("[green]100%[/]"));

            //table.UpdateCell(1, 1, new Markup("[blue]In Progress[/]"));
            //table.UpdateCell(1, 2, new Markup("[blue]0%[/]"));

            //// Insert a new row
            //table.InsertRow(3, new Markup("Task 4"), new Markup("[yellow]Pending[/]"), new Markup("0%"));

            //var table2 = new Table()
            //    .AddColumn("Server")
            //    .AddColumn("Status")
            //    .AddColumn("Uptime");

            //int frame = 0;
            //AnsiConsole.Live(table)
            //    .Start(ctx =>
            //    {
            //        while (true)
            //        {
            //            table.UpdateCell(1, 2, new Markup($"[blue]{frame}%[/]"));
            //            frame++;

            //            if (frame == 100)
            //            {
            //                table.UpdateCell(1, 1, new Markup("[green]Complete[/]"));
            //                table.UpdateCell(1, 2, new Markup("[green]100%[/]"));

            //                ctx.UpdateTarget(table2);
            //                return;
            //            }
            //            Thread.Sleep(100);
            //            ctx.Refresh();
            //        }
            //    });


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