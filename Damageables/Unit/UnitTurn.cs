public struct UnitTurn
{
    public UnitTurn(Unit unit, bool isAlly, bool canTurn)
    {
        _unit = unit;
        _isAlly = isAlly;
        _canTurn = canTurn;
    }

    public Unit Unit => _unit;
    public bool IsAlly => _isAlly;
    public bool IsAllowed => _canTurn && _unit.IsAlive;

    private Unit _unit;
    private bool _isAlly;
    private bool _canTurn;
}