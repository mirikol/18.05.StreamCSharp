internal class Program
{
    private static void Main()
    {
        try
        {
            Logger.enabled = true;

            //var goldenSword = new Sword("Golden sword", 0, 10, 200);
            //var commonSword = new Sword("Common sword", 0, 5, 100);

            //var glove = new Glove("Glove", 10, 0);
            //var greave = new Greave("Greave", 20, 5);
            //var helmet = new Helmet("Helmet", 0, 10);
            //var vest = new Vest("Vest", 100, 0);

            //SaveLoad<IWeapon>.Save(goldenSword, goldenSword.Name);
            //SaveLoad<IWeapon>.Save(commonSword, commonSword.Name);

            //SaveLoad<IArmor>.Save(glove, glove.Name);
            //SaveLoad<IArmor>.Save(greave, greave.Name);
            //SaveLoad<IArmor>.Save(helmet, helmet.Name);
            //SaveLoad<IArmor>.Save(vest, vest.Name);

            //var unit1 = new Unit(SaveLoad<UnitModel>.Load("Timur"));
            //EquipUtility.EquipUnit(unit1, goldenSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit1, vest, BodyPartName.Body);
            //EquipUtility.EquipUnit(unit1, helmet, BodyPartName.Head);
            //EquipUtility.EquipUnit(unit1, glove, BodyPartName.RightArm);
            //var unitSave1 = new UnitSave(unit1);
            //SaveLoad<UnitSave>.Save(unitSave1, "Player");

            //var unit2 = new Unit(SaveLoad<UnitModel>.Load("Gregory"));
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.LeftArm);
            //var unitSave2 = new UnitSave(unit2);
            //SaveLoad<UnitSave>.Save(unitSave2, "Gregory");

            //var unit3 = new Unit(SaveLoad<UnitModel>.Load("Michael"));
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.LeftArm);
            //var unitSave3 = new UnitSave(unit3);
            //SaveLoad<UnitSave>.Save(unitSave3, "Michael");

            Arena arena = new Arena();
            arena.Start();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }
}