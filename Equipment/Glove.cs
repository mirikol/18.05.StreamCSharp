public class Glove : IArmor, IArmEquipment
{
    public int Defense => _defense;
    public int Attack => _attack;

    private int _defense;
    private int _attack;

    public Glove(int defense, int attack)
    {
        _defense = defense;
        _attack = attack;
    }
}