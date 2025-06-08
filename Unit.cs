public class Unit : IDamageable
{
    public event Action HasDied;

    public bool IsAlive => _model.Health > 0;

    private UnitModel _model;
    public UnitModel Model => _model;

    public Unit(UnitModel model)
    {
        _model = model;
    }

    public void TakeDamage(int damage)
    {
        _model.Health -= damage;
        Printer.Print($"{_model.Name} health = {_model.Health}", ConsoleColor.Yellow);

        if (_model.Health <= 0)
        {
            Printer.Print($"{_model.Name} died", ConsoleColor.DarkRed);
            HasDied?.Invoke();
        }

        Console.WriteLine();
    }
}