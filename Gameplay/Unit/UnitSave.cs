using System.Text.Json.Serialization;

public class UnitSave
{
    public List<ArmorSave> Armors { get; private set; }
    public List<WeaponSave> Weapons { get; private set; }

    public string UnitModelName { get; private set; }

    public UnitModel UnitModel => _unitModel;
    private UnitModel _unitModel;

    [JsonConstructor]
    public UnitSave(List<ArmorSave> armors, List<WeaponSave> weapons, string unitModelName)
    {
        Armors = new List<ArmorSave>(armors);
        Weapons = new List<WeaponSave>(weapons);
        UnitModelName = unitModelName;

        _unitModel = SaveLoad<UnitModel>.Load(UnitModelName);
    }
}

public struct ArmorSave
{
    public string ArmorName { get; private set; }
    public BodyPartName BodyPart { get; private set; }

    private IArmor _armor;
    public IArmor Armor => _armor;

    [JsonConstructor]
    public ArmorSave(string armorName, BodyPartName bodyPart)
    {
        ArmorName = armorName;
        BodyPart = bodyPart;

        _armor = SaveLoad<IArmor>.Load(armorName);
    }
}

public struct WeaponSave
{
    public string WeaponName { get; private set; }
    public BodyPartName BodyPart { get; private set; }

    private IWeapon _weapon;
    public IWeapon Weapon => _weapon;

    [JsonConstructor]
    public WeaponSave(string weaponName, BodyPartName bodyPart)
    {
        WeaponName = weaponName;
        BodyPart = bodyPart;

        _weapon = SaveLoad<IWeapon>.Load(weaponName);
    }
}