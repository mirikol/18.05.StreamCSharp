using System.Text.Json.Serialization;

public class ArenaModel
{
    public List<string> PlayerUnitsName { get; private set; }
    public List<string> EnemyUnitsName { get; private set; }

    private List<Unit> _playerUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    public int[][] PlayerUnitsPlacements { get; private set; }
    public int[][] EnemyUnitsPlacements { get; private set; }

    public IReadOnlyCollection<Unit> PlayerUnits => _playerUnits;
    public IReadOnlyCollection<Unit> EnemyUnits => _enemyUnits;

    [JsonConstructor]
    public ArenaModel(List<string> playerUnitsName, List<string> enemyUnitsName, int[][] playerUnitsPlacements, int[][] enemyUnitsPlacements)
    {
        for (int i = 0; i < playerUnitsName.Count; i++)
        {
            _playerUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load(playerUnitsName[i]), playerUnitsPlacements[i]));
        }
        for (int i = 0; i < enemyUnitsName.Count; i++)
        {
            _enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load(enemyUnitsName[i]), enemyUnitsPlacements[i]));
        }
    }
}