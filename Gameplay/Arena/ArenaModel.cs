using System.Text.Json.Serialization;

public class ArenaModel
{
    public List<string> PlayerUnitsName { get; private set; }
    public List<string> EnemyUnitsName { get; private set; }

    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    public IReadOnlyCollection<Unit> PlayerUnits => _playerUnits;
    public IReadOnlyCollection<Unit> EnemyUnits => _enemyUnits;

    [JsonConstructor]
    public ArenaModel(List<string> playerUnitsName, List<string> enemyUnitsName)
    {
        foreach (var unitName in playerUnitsName)
        {
            _playerUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load(unitName)));
        }
        foreach (var unitName in enemyUnitsName)
        {
            _enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load(unitName)));
        }
    }
}