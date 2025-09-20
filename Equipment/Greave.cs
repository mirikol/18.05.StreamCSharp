public class Greave : IArmor, ILegEquipment
{
    public string Name => _name;
    public int Defense => _defense;
    public int Attack => _attack;

    private string _name;
    private int _defense;
    private int _attack;

    public Greave(string name, int defense, int attack)
    {
        _name = name;
        _defense = defense;
        _attack = attack;
    }
}