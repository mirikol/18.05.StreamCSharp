using Spectre.Console;

public class UnitsPrinter : IPrinter
{
    private const int gridColumns = 3;
    private const int gridsCount = 9;
    private const int bigPanelHeight = 6;
    private const int normalPanelHeight = 3;
    private const int panelWidth = 25;

    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Grid _grid;

    private List<Panel> _panels = new List<Panel>();
    private List<string> _unitNames = new List<string>();
    private int _selectIndex = 0;
    private Color _lastColor = Color.White;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
    }

    public string GetSelectedUnitName()
    {
        return _unitNames[_selectIndex];
    }

    public void ResetSelect()
    {
        Select(0);
    }

    public void SelectUp()
    {
        int index = _selectIndex - 1;

        if ((_selectIndex + CountBigUnitCellsBetween(0, _selectIndex, false)) % 2 == 0 && _selectIndex > 6)
        {
            index = _selectIndex - 12 + CountBigUnitCellsBetween(_selectIndex - 12, _selectIndex, true);
        }

        Select(index);
    }

    public void SelectDown()
    {
        int index = _selectIndex + 1;

        if ((_selectIndex + CountBigUnitCellsBetween(0, _selectIndex, true)) % 2 == 1 && _selectIndex < 6)
        {
            index = _selectIndex + 12 - CountBigUnitCellsBetween(_selectIndex, _selectIndex + 12, false);
        }

        Select(index);
    }

    public void SelectLeft()
    {
        int index = _selectIndex - 2;

        if (CheckBigUnitCell(_selectIndex - 1))
        {
            index = _selectIndex - 1;
        }

        Select(index);
    }

    public void SelectRight()
    {
        int index = _selectIndex + 2;

        if (CheckBigUnitCell(_selectIndex + 1))
        {
            index = _selectIndex + 1;
        }

        Select(index);
    }

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index <= _panels.Count - 1 && !CheckNoUnitCell(index);
    }

    private int CountBigUnitCellsBetween(int startIndex, int endIndex, bool includeSelf)
    {
        int count = 0;

        if (includeSelf)
        {
            endIndex++;
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            if (IsIndexValid(i) && CheckBigUnitCell(i) && !CheckNoUnitCell(i))
            {
                count++;
            }
        }

        return count;
    }

    private bool CheckBigUnitCell(int index)
    {
        return IsIndexValid(index) && _panels[index].Height == bigPanelHeight;
    }

    private bool CheckNoUnitCell(int index)
    {
        return _panels[index].Header?.Text == "Field";
    }
    private bool CheckEmptyCell(int index)
    {
        return _panels[index].Header?.Text == "";
    }

    private void Select(int index)
    {
        if (!IsIndexValid(index))
        {
            return;
        }

        if (CheckEmptyCell(_selectIndex))
        {
            _panels[_selectIndex].BorderColor(Color.Gray);
        }
        else
        {
            _panels[_selectIndex].BorderColor(_lastColor);
        }
        _selectIndex = Math.Clamp(index, 0, _panels.Count - 1);
        if (CheckEmptyCell(_selectIndex))
        {
            _panels[_selectIndex].BorderColor(Color.Red);
        }
        else
        {
            _lastColor = _panels[_selectIndex].BorderStyle.Foreground;
            _panels[_selectIndex].BorderColor(Color.Green);
        }
        _displayContext.Refresh();
    }

    public void Print(UnitsContext context)
    {
        _panels.Clear();
        _unitNames.Clear();

        _grid = new Grid();

        for (int i = 0; i < gridColumns; i++)
        {
            _grid.AddColumn();
        }

        Grid[] panels = new Grid[gridsCount];

        Dictionary<int, Dictionary<int, string>> indeciesToName = new Dictionary<int, Dictionary<int, string>>();
        string unitTurnName = "";

        for (int i = 0; i < context.UnitsPlacement.Length; i++)
        {
            foreach (var globalIndex in context.UnitsPlacement[i])
            {
                int gridIndex = GetGridIndex(globalIndex, context.Units[i].IsAlly);
                int panelIndex = GetPanelIndex(globalIndex, context.Units[i].IsAlly);

                if (!indeciesToName.ContainsKey(gridIndex))
                {
                    indeciesToName[gridIndex] = new Dictionary<int, string>();
                }

                if (context.Units[i].Unit == context.UnitTurn.Unit)
                {
                    unitTurnName = context.UnitTurn.Unit.Model.Name;
                }

                if (!indeciesToName[gridIndex].ContainsKey(panelIndex))
                {
                    indeciesToName[gridIndex].Add(panelIndex, context.Units[i].Unit.Model.Name);
                }
            }
        }

        for (int i = 0; i < gridsCount; i++)
        {
            if (i >= 3 && i <= 5)
            {
                panels[i] = new Grid();
                panels[i].AddColumn();

                var panel1 = new Panel("") { Height = bigPanelHeight }.NoBorder().Header("Field");
                var panel2 = new Panel("") { Height = bigPanelHeight }.NoBorder().Header("Field");

                panels[i].AddRow(panel1);
                panels[i].AddRow(panel2);

                _unitNames.Add("");
                _unitNames.Add("");

                _panels.Add(panel1);
                _panels.Add(panel2);
            }
            else
            {
                panels[i] = new Grid();
                panels[i].AddColumn();

                if (indeciesToName.ContainsKey(i))
                {
                    if (indeciesToName[i].ContainsKey(0) && indeciesToName[i].ContainsKey(1))
                    {
                        if (indeciesToName[i][0] == indeciesToName[i][1])
                        {
                            var panel3 = new Panel(indeciesToName[i][0]) { Height = bigPanelHeight, Width = panelWidth }.BorderColor(indeciesToName[i][0] == unitTurnName ? Color.Cyan : Color.White);
                            panels[i].AddRow(panel3);

                            _unitNames.Add(indeciesToName[i][0]);
                            _panels.Add(panel3);
                        }
                        else
                        {
                            var panel4 = new Panel(indeciesToName[i][0]) { Height = normalPanelHeight, Width = panelWidth }.BorderColor(indeciesToName[i][0] == unitTurnName ? Color.Cyan : Color.White);
                            var panel5 = new Panel(indeciesToName[i][1]) { Height = normalPanelHeight, Width = panelWidth }.BorderColor(indeciesToName[i][1] == unitTurnName ? Color.Cyan : Color.White);

                            panels[i].AddRow(panel4);
                            panels[i].AddRow(panel5);

                            _unitNames.Add(indeciesToName[i][0]);
                            _unitNames.Add(indeciesToName[i][1]);

                            _panels.Add(panel4);
                            _panels.Add(panel5);
                        }
                    }
                    else if (!indeciesToName[i].ContainsKey(1))
                    {
                        var panel6 = new Panel(indeciesToName[i][0]) { Height = normalPanelHeight, Width = panelWidth }.BorderColor(indeciesToName[i][0] == unitTurnName ? Color.Cyan : Color.White);
                        var panel7 = new Panel("Empty") { Height = normalPanelHeight, Width = panelWidth }.BorderColor(Color.Gray).Header("");

                        panels[i].AddRow(panel6);
                        panels[i].AddRow(panel7);

                        _unitNames.Add(indeciesToName[i][0]);
                        _unitNames.Add("");

                        _panels.Add(panel6);
                        _panels.Add(panel7);
                    }
                    else
                    {
                        var panel8 = new Panel("Empty") { Height = normalPanelHeight, Width = panelWidth }.BorderColor(Color.Gray).Header("");
                        var panel9 = new Panel(indeciesToName[i][1]) { Height = normalPanelHeight, Width = panelWidth }.BorderColor(indeciesToName[i][1] == unitTurnName ? Color.Cyan : Color.White);

                        panels[i].AddRow(panel8);
                        panels[i].AddRow(panel9);

                        _unitNames.Add("");
                        _unitNames.Add(indeciesToName[i][1]);

                        _panels.Add(panel8);
                        _panels.Add(panel9);
                    }
                }
                else
                {
                    var panel10 = new Panel("Empty") { Height = normalPanelHeight, Width = panelWidth }.BorderColor(Color.Gray).Header("");
                    var panel11 = new Panel("Empty") { Height = normalPanelHeight, Width = panelWidth }.BorderColor(Color.Gray).Header("");

                    panels[i].AddRow(panel10);
                    panels[i].AddRow(panel11);

                    _unitNames.Add("");
                    _unitNames.Add("");

                    _panels.Add(panel10);
                    _panels.Add(panel11);
                }
            }
        }

        for (int i = 0; i <= gridsCount - 1; i += 3)
        {
            _grid.AddRow(panels[i], panels[i + 1], panels[i + 2]);
        }

        var panel = new Panel(_grid).Header("Units").Expand();
        _layout["Battle"]["Turn"]["Units"].Update(panel);
        _displayContext.Refresh();

        _lastColor = _panels[_selectIndex].BorderStyle.Foreground;
    }

    private int GetGridIndex(int index, bool isAlly)
    {
        if (isAlly)
        {
            if (index >= 3)
            {
                return index + 3;
            }
            else
            {
                return index + 6;
            }
        }

        else
        {
            if (index >= 3)
            {
                return index - 3;
            }
            else
            {
                return index;
            }
        }
    }

    private int GetPanelIndex(int index, bool isAlly)
    {
        if (isAlly)
        {
            if (index >= 3)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        else
        {
            if (index >= 3)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}