public class Arena
{
    private GameplayLogPrinter _gameplayLogPrinter;
    private UnitsPrinter _unitsPrinter;

    private TurnController _turnController;
    private ArenaModel _model;

    private BattleState _state;
    public BattleState State => _state;

    public Arena(GameplayLogPrinter gameplayLogPrinter, UnitsPrinter unitsPrinter, StatsPrinter statsPrinter, VitalsPrinter vitalsPrinter, SkillsPrinter skillsPrinter, ArenaModel model, SkillMenu skillMenu)
    {
        _gameplayLogPrinter = gameplayLogPrinter;
        _unitsPrinter = unitsPrinter;

        _model = model;
        _turnController = new TurnController(_gameplayLogPrinter, _unitsPrinter, statsPrinter, vitalsPrinter, skillsPrinter, new TurnPrinter(_gameplayLogPrinter, _unitsPrinter), _model.PlayerUnits, _model.EnemyUnits, skillMenu);
    }

    public void Start()
    {
        while (true)
        {
            var nextTurn = _turnController.GetNextTurn();

            var battleState = _turnController.GetBattleState();
            if (battleState != BattleState.Battle)
            {
                _state = battleState;
                return;
            }

            _turnController.Turn(nextTurn);
        }
    }
}