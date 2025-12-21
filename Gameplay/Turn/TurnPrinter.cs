public class TurnPrinter
{
    private const ConsoleColor _playerColor = ConsoleColor.Green;
    private const ConsoleColor _enemyColor = ConsoleColor.DarkCyan;
    private const ConsoleColor _diedUnitColor = ConsoleColor.Red;

    public void Print(UnitTurn[] units, UnitTurn unitTurn)
    {
        var color = GetPrintColor(unitTurn);

        PrintTurnTitle(unitTurn, color);
        PrintUnits(units, unitTurn, color);

        Console.WriteLine();
    }

    private void PrintUnits(UnitTurn[] units, UnitTurn unitTurn, ConsoleColor color)
    {
        for (int i = 0; i < units.Length; i++)
        {
            string unitName = units[i].Unit.Model.Name;

            if (i == unitTurn.Order)
            {
                unitName += " <<<";
            }

            if (units[i].Unit.IsAlive)
            {
                Printer.Print(unitName, color);
            }
            else
            {
                Printer.Print(unitName + " [DEAD]", _diedUnitColor);
            }
        }
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
            Printer.Print("[YOUR TURN]", color);
        }
        else
        {
            Printer.Print("[ENEMY TURN]", color);
        }
    }
}