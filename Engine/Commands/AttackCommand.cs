public class AttackCommand : ICommand
{
    private GameplayLogPrinter _printer;
    private Unit _attacker;
    private Unit _defender;
    private BodyPartName _bodyPart;
    private int _damage;
    private float _probability;

    public AttackCommand(GameplayLogPrinter printer, Unit attacker, Unit defender, BodyPartName bodyPart, int damage, float probability)
    {
        _printer = printer;
        _attacker = attacker;
        _defender = defender;
        _bodyPart = bodyPart;
        _damage = damage;
        _probability = probability / 100f;
    }

    public void Execute()
    {
        Random random = new Random();
        LogContext context = new LogContext($"{_attacker.Model.Name} атаковал {_defender.Model.Name} с уроном {_damage}.", ConsoleColor.DarkRed);
        _printer.Print(context);

        if (_probability > random.NextDouble())
        {
            _defender.BodyParts[_bodyPart].TakeDamage(_damage);
        }
        else
        {
            context.Text = "Промазал\n";
            context.ForegroundColor = ConsoleColor.DarkRed;
            _printer.Print(context);
        }
    }
}