public class UnitModel
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Initiative { get; private set; }
    public int Speed { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public UnitModel(string name, int health, int initiative, int speed, int attack, int defense)
    {
        Name = name;
        Health = health;

        Initiative = initiative;
        if (initiative < 1)
        {
            throw new ArgumentException($"Unit {name} has spawned with initiative = {initiative}");
        }

        Speed = speed;
        if (speed < 1)
        {
            throw new ArgumentException($"Unit {name} has spawned with speed = {speed}");
        }

        Attack = attack;
        if (attack < 1)
        {
            throw new ArgumentException($"Unit {name} has spawned with attack = {attack}");
        }

        Defense = defense;
        if (defense < 1)
        {
            throw new ArgumentException($"Unit {name} has spawned with defense = {defense}");
        }
    }
}