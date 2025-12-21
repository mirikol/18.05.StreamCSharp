public class BattleProcessor
{
    private TurnPrinter _printer;

    public BattleProcessor(TurnPrinter printer)
    {
        _printer = printer;
    }

    public void Battle(UnitTurn[] turnCycle, UnitTurn[] enemies, UnitTurn[] allies, UnitTurn attackerTurn, Action onComplete)
    {
        _printer.Print(turnCycle, attackerTurn);

        Unit enemy;
        if (attackerTurn.IsAlly)
        {
            enemy = SelectEnemy(enemies, true);
        }
        else
        {
            enemy = SelectEnemy(allies, false);
        }

        BodyPartName bodyPart = SelectBodyPart(attackerTurn.IsAlly);
        int attackIndex = SelectAttack(attackerTurn.Unit, enemy, bodyPart, attackerTurn.IsAlly);

        for (int i = 0; i < turnCycle.Length; i++)
        {
            if (turnCycle[i].Order == attackerTurn.Order)
            {
                turnCycle[i] = new UnitTurn(attackerTurn.Unit, attackerTurn.IsAlly, false, i);
                break;
            }
        }

        onComplete();
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
}