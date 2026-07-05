public struct UnitsContext
{
    public UnitTurn[] Units;
    public int[][] UnitsPlacement;
    public UnitTurn UnitTurn;

    public UnitsContext(UnitTurn[] units, int[][] unitsPlacement, UnitTurn unitTurn)
    {
        Units = units;
        UnitsPlacement = unitsPlacement;
        UnitTurn = unitTurn;
    }

    public UnitsContext(UnitTurn[] units, UnitTurn unitTurn)
    {
        Units = units;
        UnitTurn = unitTurn;
    }
}