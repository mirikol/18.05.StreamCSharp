public class Greave : IArmor, ILegEquipment
{
    public int Defense => _defense;
    public int Attack => _attack;

    private int _defense;
    private int _attack;

    public Greave(int defense, int attack)
    {
        _defense = defense;
        _attack = attack;
    }
}