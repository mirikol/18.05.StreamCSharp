public class AttackCommand : ICommand
{
    private const float _attackModifier = 0.6f;

    private Unit _attacker;
    private Unit _defender;
    private BodyPartName _bodyPart;
    private int _damage;
    private float _probability;

    public AttackCommand(Unit attacker, Unit defender, BodyPartName bodyPart, int damage, float probability)
    {
        _attacker = attacker;
        _defender = defender;
        _bodyPart = bodyPart;
        _damage = (int)(damage * (1 + (_attacker.Model.Attack * _attackModifier) / (float)((_attacker.Model.Attack * _attackModifier) + _defender.Model.Defense)));
        _probability = probability / 100f;
    }

    public void Execute()
    {
        Random random = new Random();
        Printer.Print($"{_attacker.Model.Name} атаковал {_defender.Model.Name}", ConsoleColor.DarkRed);

        if (_probability > random.NextDouble())
            _defender.BodyParts[_bodyPart].TakeDamage(_damage);
        else
            Printer.Print("Промазал\n", ConsoleColor.DarkRed);
    }
}
