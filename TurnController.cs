public class TurnController
{
    private const ConsoleColor _playerColor = ConsoleColor.Green;
    private const ConsoleColor _enemyColor = ConsoleColor.DarkCyan;

    public Unit[] TurnCycle => _turnCycle.ToArray();
    private List<Unit> _turnCycle = new List<Unit>();
    private List<Unit> _allUnits = new List<Unit>();
    private List<Unit> _deadUnits = new List<Unit>();

    private Unit? _previousUnit = null;
    private int _oneUnitTurnIndex = 0;

    public TurnController(List<Unit> units)
    {
        _allUnits = new List<Unit>(units);
        foreach (var unit in _allUnits)
        {
            unit.HealthBelowZero += () =>
            {
                DeleteFromTurnCycle(unit);
                _deadUnits.Add(unit);
            };
        }

        CreateTurnCycle();
    }

    public bool CanTurn(Unit turningUnit) => !_deadUnits.Contains(turningUnit);

    public void Update()
    {
        List<Unit> deadUnits = new List<Unit>();
        foreach (var unit in _allUnits)
        {
            if (!_turnCycle.Contains(unit))
            {
                deadUnits.Add(unit);
            }
        }

        _turnCycle.Clear();
        _previousUnit = null;
        _oneUnitTurnIndex = 0;

        CreateTurnCycle();
        foreach (var unit in deadUnits)
        {
            while (_turnCycle.Contains(unit))
            {
                _turnCycle.Remove(unit);
            }
        }
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

    private int SelectAttack(Unit attacker, Unit defender, BodyPartName bodyPart, bool playerTurn)
    {
        List<CommandBinding> bindings =
        [
            new CommandBinding($"Weak: {UnitUtility.GetFlatDamage(attacker.BaseDamage, attacker, defender)} damage (90%)", new AttackCommand(attacker, defender, bodyPart, UnitUtility.GetFlatDamage(attacker.BaseDamage, attacker, defender), 90)),
            new CommandBinding($"Medium: {UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 1.25f), attacker, defender)} damage (75%)", new AttackCommand(attacker, defender, bodyPart, UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 1.25f), attacker, defender), 75)),
            new CommandBinding($"Strong: {UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 2f), attacker, defender)} damage (50%)", new AttackCommand(attacker, defender, bodyPart, UnitUtility.GetFlatDamage((int)(attacker.BaseDamage * 2f), attacker, defender), 50)),
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
        var color = GetPrintColor(playerTurn);

        CheckUnitRepeat(turnUnit);
        PrintTurnTitle(playerTurn, color);
        PrintUnits(turnUnit, color);

        Console.WriteLine();
    }

    private void PrintUnits(Unit turnUnit, ConsoleColor color)
    {
        int index = _oneUnitTurnIndex;
        bool turnUnitSelected = false;

        for (int i = 0; i < _turnCycle.Count; i++)
        {
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
        _oneUnitTurnIndex = index;
    }

    private ConsoleColor GetPrintColor(bool playerTurn)
    {
        if (playerTurn)
        {
            return _playerColor;
        }
        else
        {
            return _enemyColor;
        }

    }

    private void PrintTurnTitle(bool playerTurn, ConsoleColor color)
    {
        if (playerTurn)
        {
            Printer.Print("[YOUR TURN]", color);
        }
        else
        {
            Printer.Print("[ENEMY TURN]", color);
        }
    }

    private bool CheckUnitRepeat(Unit checkUnit)
    {
        bool unitHasRepeat = checkUnit == _previousUnit;
        if (unitHasRepeat)
        {
            _oneUnitTurnIndex++;
        }
        else
        {
            _oneUnitTurnIndex = 0;
        }

        return unitHasRepeat;
    }

    private void CreateTurnCycle()
    {
        List<(Unit, float)> tempResult = new List<(Unit, float)>();

        List<Unit> sortedByInitiativeUnits = new List<Unit>();
        List<(Unit, float)> unitsSpeed = new List<(Unit, float)>();

        foreach (Unit unit in _allUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }

        sortedByInitiativeUnits = sortedByInitiativeUnits.OrderByDescending(unit => unit.Initiative).ToList();
        float minSpeed = sortedByInitiativeUnits.OrderByDescending(unit => unit.Speed).Last().Speed;

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            float speed = unit.Speed / minSpeed;

            while (speed > 0)
            {
                unitsSpeed.Add((unit, speed));
                speed -= (float)Math.Cbrt(unit.Speed);
            }
        }

        unitsSpeed = unitsSpeed.OrderByDescending(x => x.Item2).ToList();
        List<List<(Unit, float)>> unitsWithSpeed = new List<List<(Unit, float)>>();

        foreach (Unit unit in sortedByInitiativeUnits)
        {
            List<(Unit, float)> unitList = new List<(Unit, float)>();

            tempResult.Add((unit, unit.Speed / minSpeed));
            unitsSpeed.Remove((unit, unit.Speed / minSpeed));

            foreach (var speedUnit in unitsSpeed)
            {
                if (speedUnit.Item1 == unit)
                {
                    unitList.Add(speedUnit);
                }
            }

            unitsWithSpeed.Add(unitList);
        }

        for (int i = 0; i < unitsWithSpeed.Count; i++)
        {
            if (unitsWithSpeed[i].Count == 0)
            {
                continue;
            }

            int startIndex = 0;
            foreach (var unit in tempResult)
            {
                if (unit.Item1 == unitsWithSpeed[i][0].Item1)
                {
                    break;
                }
                startIndex++;
            }

            foreach (var speedUnit in unitsWithSpeed[i])
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