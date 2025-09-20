using System.Text.Json.Serialization;

public class UnitSave
{
    public List<ArmorSave> Armors { get; private set; }
    public List<WeaponSave> Weapons { get; private set; }

    public UnitModel UnitModel { get; private set; }

    [JsonConstructor]
    public UnitSave(List<ArmorSave> armors, List<WeaponSave> weapons, UnitModel unitModel)
    {
        Armors = new List<ArmorSave>(armors);
        Weapons = new List<WeaponSave>(weapons);
        UnitModel = unitModel;
    }

    public UnitSave(Unit unit)
    {
        UnitModel = unit.Model;

        Armors = new List<ArmorSave>();
        Weapons = new List<WeaponSave>();

        foreach (var bodyPart in unit.BodyParts)
        {
            if (bodyPart.Value.Armor != null)
            {
                var armorSave = new ArmorSave(bodyPart.Value.Armor, bodyPart.Key);
                Armors.Add(armorSave);
            }

            if (bodyPart.Value is Arm arm)
            {
                if (arm.Weapon != null)
                {
                    var weaponSave = new WeaponSave(arm.Weapon, bodyPart.Key);
                    Weapons.Add(weaponSave);
                }
            }
        }
    }
}

public struct ArmorSave
{
    public IArmor Armor { get; private set; }
    public BodyPartName BodyPart { get; private set; }

    [JsonConstructor]
    public ArmorSave(IArmor armor, BodyPartName bodyPart)
    {
        Armor = armor;
        BodyPart = bodyPart;
    }
}

public struct WeaponSave
{
    public IWeapon Weapon { get; private set; }
    public BodyPartName BodyPart { get; private set; }

    [JsonConstructor]
    public WeaponSave(IWeapon weapon, BodyPartName bodyPart)
    {
        Weapon = weapon;
        BodyPart = bodyPart;
    }
}