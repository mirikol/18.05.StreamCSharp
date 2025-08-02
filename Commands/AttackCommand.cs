using static System.Net.Mime.MediaTypeNames;

public class AttackCommand : ICommand
{
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
        _damage = damage;
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