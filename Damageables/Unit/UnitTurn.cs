public struct UnitTurn
{
    public UnitTurn(Unit unit, bool isAlly, bool canTurn, int order)
    {
        _unit = unit;
        _isAlly = isAlly;
        _canTurn = canTurn;
        _order = order;
    }

    public Unit Unit => _unit;
    public bool IsAlly => _isAlly;
    public bool IsAllowed => _canTurn && _unit.IsAlive;
    public int Order => _order;

    private Unit _unit;
    private bool _isAlly;
    private bool _canTurn;
    private int _order;
}