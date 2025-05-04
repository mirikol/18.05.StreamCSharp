
public class Unit : IDamageable
{
    public event Action HasDied;

    private int _health;
    public int Health => _health;

    private string _name;
    public string Name => _name;

    public int _initiative;
    public int Initiative => _initiative;

    public Unit(string name, int health, int initiative)
    {
        _name = name;
        _health = health;
        _initiative = initiative;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Printer.Print($"{_name} health = {_health}", ConsoleColor.Yellow);

        if (_health <= 0)
        {
            Printer.Print($"{_name} died", ConsoleColor.DarkRed);
            HasDied?.Invoke();
        }

        Console.WriteLine();
    }
}
