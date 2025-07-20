using System;

public class TurnController
{
    public Unit[] TurnCycle => _turnCycle.ToArray();
    private List<Unit> _turnCycle = new List<Unit>();

    private Unit _previousUnit = null;
    private int _oneUnitTurnIndex = 0;

    public TurnController(Arena arena, List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        arena.UnitHasDied += DeleteFromTurnCycle;
        CreateTurnCycle(playerUnits, enemyUnits);
    }

    public void Turn(Unit attacker, List<Unit> defenders, bool playerTurn)
    {
        Print(attacker, playerTurn);

        Unit enemy = SelectEnemy(defenders, playerTurn);
        BodyPartName bodyPart = SelectBodyPart(playerTurn);
        int attackIndex = SelectAttack(attacker, enemy, bodyPart, playerTurn);
    }

    private Unit SelectEnemy(List<Unit> defenders, bool playerTurn)
    {
        List<CommandBinding> bindings = new List<CommandBinding>();
        foreach (var defender in defenders)
        {
            CommandBinding binding = new CommandBinding(defender.Model.Name, new NullCommand());
            bindings.Add(binding);
        }
        int selectedEnemyIndex = GetMenuChoice("Select enemy", bindings, playerTurn);
        var enemy = defenders[selectedEnemyIndex];

        return enemy;
    }

    private BodyPartName SelectBodyPart(bool playerTurn)
    {
        List<CommandBinding> bindings = new List<CommandBinding>();
        var bodyPartNames = Enum.GetNames(typeof(BodyPartName));
        foreach (var bodyPartName in bodyPartNames)
        {
            CommandBinding binding = new CommandBinding(bodyPartName, new NullCommand());
            bindings.Add(binding);
        }
        int selectedBodyPartIndex = GetMenuChoice("Select body part", bindings, playerTurn);
        BodyPartName bodyPart = (BodyPartName)Enum.Parse(typeof(BodyPartName), bodyPartNames[selectedBodyPartIndex]);

        return bodyPart;
    }

    private int SelectAttack(Unit attacker, Unit enemy, BodyPartName bodyPart, bool playerTurn)
    {
        List<CommandBinding> bindings =
        [
            new CommandBinding($"Weak: {50} damage (90%)", new AttackCommand(attacker, enemy, bodyPart, 50, 90)),
            new CommandBinding($"Medium: {(int)(50 * 1.25f)} damage (75%)", new AttackCommand(attacker, enemy, bodyPart, (int)(50 * 1.25f), 75)),
            new CommandBinding($"Strong: {(int)(50 * 2f)} damage (50%)", new AttackCommand(attacker, enemy, bodyPart, (int)(50 * 2f), 50)),
        ];
        int selectedAttackIndex = GetMenuChoice("Select attack", bindings, playerTurn);

        return selectedAttackIndex;
    }

    private int GetMenuChoice(string menuName, List<CommandBinding> bindings, bool playerTurn)
    {
        var menu = new Menu(menuName, bindings.ToArray());

        int selectIndex;
        if (playerTurn)
        {
            menu.Show();
            selectIndex = menu.GetInput();
        }
        else
        {
            Random random = new Random();
            selectIndex = random.Next(1, menu.BindingsCount + 1);
        }
        menu.Select(selectIndex);

        return selectIndex - 1;
    }

    private void Print(Unit turnUnit, bool playerTurn)
    {
        bool turnUnitSelected = false;
        ConsoleColor color;

        if (turnUnit == _previousUnit)
        {
            _oneUnitTurnIndex++;
        }
        else
        {
            _oneUnitTurnIndex = 0;
        }

        if (playerTurn)
        {
            color = ConsoleColor.Green;
            Printer.Print("[YOUR TURN]", color);
        }
        else
        {
            color = ConsoleColor.DarkCyan;
            Printer.Print("[ENEMY TURN]", color);
        }

        for (int i = 0; i < _turnCycle.Count; i++)
        {
            if (!_turnCycle[i].IsAlive)
            {
                continue;
            }

            string unitName = _turnCycle[i].Model.Name;
            if (turnUnit == _turnCycle[i])
            {
                if (_oneUnitTurnIndex == 0)
                {
                    if (!turnUnitSelected)
                    {
                        unitName += " <<<";
                        turnUnitSelected = true;
                    }
                }
                else
                {
                    _oneUnitTurnIndex--;
                }
            }

            Printer.Print(unitName, color);
        }

        _previousUnit = turnUnit;
        Console.WriteLine();
    }

    private void CreateTurnCycle(List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        List<(Unit, float)> tempResult = new List<(Unit, float)>();

        List<Unit> sortedByInitiativeUnits = new List<Unit>();
        List<(Unit, float)> unitsSpeed = new List<(Unit, float)>();

        foreach (Unit unit in playerUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }
        foreach (Unit unit in enemyUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }

        sortedByInitiativeUnits = sortedByInitiativeUnits.OrderByDescending(unit => unit.Model.Initiative).ToList();
        float minSpeed = sortedByInitiativeUnits.OrderByDescending(unit => unit.Model.Speed).Last().Model.Speed;

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            float speed = unit.Model.Speed / minSpeed;

            while (speed > 0)
            {
                unitsSpeed.Add((unit, speed));
                speed -= (float)Math.Cbrt(unit.Model.Speed);
            }
        }

        unitsSpeed = unitsSpeed.OrderByDescending(x => x.Item2).ToList();
        List<List<(Unit, float)>> units = new List<List<(Unit, float)>>();

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            List<(Unit, float)> unitList = new List<(Unit, float)>();

            tempResult.Add((unit, unit.Model.Speed / minSpeed));
            unitsSpeed.Remove((unit, unit.Model.Speed / minSpeed));

            foreach (var speedUnit in unitsSpeed)
            {
                if (speedUnit.Item1 == unit)
                {
                    unitList.Add(speedUnit);
                }
            }

            units.Add(unitList);
        }

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Count == 0)
            {
                continue;
            }

            int startIndex = 0;
            foreach (var unit in tempResult)
            {
                if (unit.Item1 == units[i][0].Item1)
                {
                    break;
                }
                startIndex++;
            }

            foreach (var speedUnit in units[i])
            {
                bool exit = false;

                for (int j = startIndex; j < tempResult.Count; j++)
                {
                    if (exit)
                    {
                        break;
                    }

                    if (speedUnit.Item2 > tempResult[j].Item2)
                    {
                        continue;
                    }
                    else
                    {
                        for (int k = j; k < tempResult.Count; k++)
                        {
                            if (speedUnit.Item2 < tempResult[k].Item2)
                            {
                                continue;
                            }
                            else
                            {
                                tempResult.Insert(Math.Max(k, 0), speedUnit);
                                exit = true;
                                break;
                            }
                        }

                        if (!exit)
                        {
                            tempResult.Add(speedUnit);
                            exit = true;
                        }
                    }
                }
            }
        }

        foreach (var unit in tempResult)
        {
            _turnCycle.Add(unit.Item1);
        }
    }

    private void DeleteFromTurnCycle(Unit unit)
    {
        _turnCycle.Remove(unit);
    }
}