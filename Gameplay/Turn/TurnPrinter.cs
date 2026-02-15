public class TurnPrinter
{
    private const ConsoleColor _playerColor = ConsoleColor.Green;
    private const ConsoleColor _enemyColor = ConsoleColor.DarkCyan;
    private const ConsoleColor _diedUnitColor = ConsoleColor.Red;

    private GameplayLogPrinter _logPrinter;
    private UnitsPrinter _unitsPrinter;

    public TurnPrinter(GameplayLogPrinter logPrinter, UnitsPrinter unitsPrinter)
    {
        _logPrinter = logPrinter;
        _unitsPrinter = unitsPrinter;
    }

    public void Print(UnitTurn[] units, UnitTurn unitTurn)
    {
        var color = GetPrintColor(unitTurn);

        PrintTurnTitle(unitTurn, color);
        PrintUnits(units, unitTurn, color);

        Console.WriteLine();
    }

    private void PrintUnits(UnitTurn[] units, UnitTurn unitTurn, ConsoleColor color)
    {
        _unitsPrinter.Print(new UnitsContext(units, units.Select(x => x.Unit.Placement).ToArray(), unitTurn));
        //for (int i = 0; i < units.Length; i++)
        //{
        //    string unitName = units[i].Unit.Model.Name;

        //    if (i == unitTurn.Order)
        //    {
        //        unitName += " <<<";
        //    }

        //    if (units[i].Unit.IsAlive)
        //    {
        //        _printer.Print(new LogContext(unitName, color));
        //    }
        //    else
        //    {
        //        _printer.Print(new LogContext(unitName + " [DEAD]", _diedUnitColor));
        //    }
        //}
    }

    private ConsoleColor GetPrintColor(UnitTurn unitTurn)
    {
        if (unitTurn.IsAlly)
        {
            return _playerColor;
        }
        else
        {
            return _enemyColor;
        }
    }

    private void PrintTurnTitle(UnitTurn unitTurn, ConsoleColor color)
    {
        if (unitTurn.IsAlly)
        {
            _logPrinter.Print(new LogContext("[YOUR TURN]", color));
        }
        else
        {
            _logPrinter.Print(new LogContext("[ENEMY TURN]", color));
        }
    }
}