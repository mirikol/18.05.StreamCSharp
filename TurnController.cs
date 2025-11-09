public class TurnController
{
    private const ConsoleColor _playerColor = ConsoleColor.Green;
    private const ConsoleColor _enemyColor = ConsoleColor.DarkCyan;
    private const ConsoleColor _diedUnitColor = ConsoleColor.Red;

    public IReadOnlyCollection<UnitTurn> TurnCycle => _turnCycle;
    public bool HasPlayerUnit => _allies.Any(turn => turn.Unit.IsAlive);
    public bool HasEnemyUnit => _enemies.Any(turn => turn.Unit.IsAlive);

    private UnitTurn[] _turnCycle;
    private UnitTurn[] _allies => Array.FindAll(_turnCycle, turn => turn.IsAlly);
    private UnitTurn[] _enemies => Array.FindAll(_turnCycle, turn => !turn.IsAlly);

    // TODO: transform to UnitTurn.Order using
    private Unit? _previousUnit = null;
    private int _oneUnitTurnIndex = 0;
    //

    public TurnController(List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        CreateTurnCycle(allyUnits, enemyUnits);
    }

    #region TurnCycle

    public UnitTurn GetNextTurn()
    {
        int startIndex = 0;
        for (int i = 0; i < _turnCycle.Length; i++)
        {
            if (_turnCycle[i].IsAllowed)
            {
                return _turnCycle[i];
            }
        }

        BeginTurnCycle();
        return GetNextTurn();

        // return false if no UnitTurn.IsAllowed in _turnCycle
        // else return true and next UnitTurn
    }

    public void BeginTurnCycle()
    {
        _previousUnit = null;
        _oneUnitTurnIndex = 0;

        for (int i = 0; i < _turnCycle.Length; i++)
        {
            _turnCycle[i] = new UnitTurn(_turnCycle[i].Unit, _turnCycle[i].IsAlly, true, i);
        }
    }

    private void UpdateTurnCycle()
    {
        List<(Unit, float)> tempResult = new List<(Unit, float)>();

        List<Unit> sortedByInitiativeUnits = new List<Unit>();
        List<(Unit, float)> unitsSpeed = new List<(Unit, float)>();

        foreach (UnitTurn unitTurn in _turnCycle)
        {
            if (!sortedByInitiativeUnits.Contains(unitTurn.Unit))
            {
                sortedByInitiativeUnits.Add(unitTurn.Unit);
            }
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

        var tempTurnCycle = new UnitTurn[tempResult.Count];
        for (int i = 0; i < tempResult.Count; i++)
        {
            bool isAlly = _allies.Any(x => x.Unit == tempResult[i].Item1);
            bool isEnemy = _enemies.Any(x => x.Unit == tempResult[i].Item1);

            if (isAlly)
            {
                tempTurnCycle[i] = new UnitTurn(tempResult[i].Item1, true, true, i);
            }
            else if (isEnemy)
            {
                tempTurnCycle[i] = new UnitTurn(tempResult[i].Item1, false, true, i);
            }
            else
            {
                throw new Exception("No unit in ally/enemy lists");
            }
        }

        for (int i = 0; i < sortedByInitiativeUnits.Count; i++)
        {
            int notAllowedTurns = 0;
            for (int j = 0; j < _turnCycle.Length; j++)
            {
                if (!_turnCycle[j].IsAllowed && _turnCycle[j].Unit == sortedByInitiativeUnits[i])
                {
                    notAllowedTurns++;
                }
            }

            for (int k = 0; k < tempTurnCycle.Length; k++)
            {
                if (notAllowedTurns > 0 && tempTurnCycle[k].Unit == sortedByInitiativeUnits[i])
                {
                    tempTurnCycle[k] = new UnitTurn(tempTurnCycle[k].Unit, tempTurnCycle[k].IsAlly, false, k);
                    notAllowedTurns--;
                }
            }
        }

        _turnCycle = tempTurnCycle;

        _previousUnit = null;
        _oneUnitTurnIndex = 0;

        // create and remember that UnitTurn.IsAllowed

        //CreateTurnCycle(_allies, _enemies);
    }

    private void CreateTurnCycle(List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        List<(Unit, float)> tempResult = new List<(Unit, float)>();

        List<Unit> sortedByInitiativeUnits = new List<Unit>();
        List<(Unit, float)> unitsSpeed = new List<(Unit, float)>();

        foreach (Unit unit in allyUnits)
        {
            sortedByInitiativeUnits.Add(unit);
        }
        foreach (Unit unit in enemyUnits)
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

        _turnCycle = new UnitTurn[tempResult.Count];
        for (int i = 0; i < tempResult.Count; i++)
        {
            bool isAlly = allyUnits.Contains(tempResult[i].Item1);
            bool isEnemy = enemyUnits.Contains(tempResult[i].Item1);

            if (isAlly)
            {
                _turnCycle[i] = new UnitTurn(tempResult[i].Item1, true, true, i);
            }
            else if (isEnemy)
            {
                _turnCycle[i] = new UnitTurn(tempResult[i].Item1, false, true, i);
            }
            else
            {
                throw new Exception("No unit in ally/enemy lists");
            }
        }
    }

    #endregion
    #region Turn

    public void Turn(UnitTurn attackerTurn)
    {
        Print(attackerTurn.Unit, attackerTurn.IsAlly);

        Unit enemy;
        if (attackerTurn.IsAlly)
        {
            enemy = SelectEnemy(_enemies, true);
        }
        else
        {
            enemy = SelectEnemy(_allies, false);
        }

        BodyPartName bodyPart = SelectBodyPart(attackerTurn.IsAlly);
        int attackIndex = SelectAttack(attackerTurn.Unit, enemy, bodyPart, attackerTurn.IsAlly);

        for (int i = 0; i < _turnCycle.Length; i++)
        {
            if (_turnCycle[i].Order == attackerTurn.Order)
            {
                _turnCycle[i] = new UnitTurn(attackerTurn.Unit, attackerTurn.IsAlly, false, i);
                break;
            }
        }

        UpdateTurnCycle();
    }

    private Unit SelectEnemy(UnitTurn[] defenders, bool playerTurn)
    {
        defenders = Array.FindAll(defenders, defender => defender.Unit.IsAlive);

        List<CommandBinding> bindings = new List<CommandBinding>();
        foreach (var defender in defenders)
        {
            CommandBinding binding = new CommandBinding(defender.Unit.Model.Name, new NullCommand());
            bindings.Add(binding);
        }
        int selectedEnemyIndex = GetMenuChoice("Select enemy", bindings, playerTurn);
        var enemy = defenders[selectedEnemyIndex];

        return enemy.Unit;
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

    #endregion
    #region Print
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

        for (int i = 0; i < _turnCycle.Length; i++)
        {
            string unitName = _turnCycle[i].Unit.Model.Name;
            if (turnUnit == _turnCycle[i].Unit)
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

            if (_turnCycle[i].Unit.IsAlive)
            {
                Printer.Print(unitName, color);
            }
            else
            {
                Printer.Print(unitName + " [DEAD]", _diedUnitColor);
            }
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

    #endregion
}