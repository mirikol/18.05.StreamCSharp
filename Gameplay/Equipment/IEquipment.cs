public interface IEquipment
{
    public string Name { get; }
    public int Defense { get; }
    public int Attack { get; }
    public int Speed { get; }
    public int Initiative { get; }
}

public interface IHeadEquipment
{
}

public interface IArmEquipment
{
}

public interface IBodyEquipment
{
}

public interface ILegEquipment
{
}