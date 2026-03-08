using Spectre.Console;
using System;

public class UnitsPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Grid _grid;

    private List<Panel> _panels = new List<Panel>();
    private int _selectIndex = 0;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
    }

    public void Select(int offset)
    {
        _panels[_selectIndex].BorderColor(Color.White);
        _selectIndex = Math.Clamp(_selectIndex + offset, 0, _panels.Count - 1);
        _panels[_selectIndex].BorderColor(Color.Green);
        _displayContext.Refresh();
    }

    public void Print(UnitsContext context)
    {
        _panels.Clear();
        _grid = new Grid();

        for (int i = 0; i < 3; i++)
        {
            _grid.AddColumn();
        }

        Grid[] panels = new Grid[9];

        Dictionary<int, Dictionary<int, string>> indeciesToName = new Dictionary<int, Dictionary<int, string>>();
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

                indeciesToName[gridIndex].Add(panelIndex, context.Units[i].Unit.Model.Name);
            }
        }

        for (int i = 0; i < 9; i++)
        {
            if (i >= 3 && i <= 5)
            {
                panels[i] = new Grid();
                panels[i].AddColumn();

                var panel1 = new Panel("") { Height = 6 }.NoBorder();
                var panel2 = new Panel("") { Height = 6 }.NoBorder();

                panels[i].AddRow(panel1);
                panels[i].AddRow(panel2);

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
                            var panel3 = new Panel(indeciesToName[i][0]) { Height = 6, Width = 25 }.BorderColor(Color.White);
                            panels[i].AddRow(panel3);
                            _panels.Add(panel3);
                        }
                        else
                        {
                            var panel4 = new Panel(indeciesToName[i][0]) { Height = 3, Width = 25 }.BorderColor(Color.White);
                            var panel5 = new Panel(indeciesToName[i][1]) { Height = 3, Width = 25 }.BorderColor(Color.White);

                            panels[i].AddRow(panel4);
                            panels[i].AddRow(panel5);

                            _panels.Add(panel4);
                            _panels.Add(panel5);
                        }
                    }
                    else if (!indeciesToName[i].ContainsKey(1))
                    {
                        var panel6 = new Panel(indeciesToName[i][0]) { Height = 3, Width = 25 }.BorderColor(Color.White);
                        var panel7 = new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White);

                        panels[i].AddRow(panel6);
                        panels[i].AddRow(panel7);

                        _panels.Add(panel6);
                        _panels.Add(panel7);
                    }
                    else
                    {
                        var panel8 = new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White);
                        var panel9 = new Panel(indeciesToName[i][1]) { Height = 3, Width = 25 }.BorderColor(Color.White);

                        panels[i].AddRow(panel8);
                        panels[i].AddRow(panel9);

                        _panels.Add(panel8);
                        _panels.Add(panel9);
                    }
                }
                else
                {
                    var panel10 = new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White);
                    var panel11 = new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White);

                    panels[i].AddRow(panel10);
                    panels[i].AddRow(panel11);

                    _panels.Add(panel10);
                    _panels.Add(panel11);
                }
            }
        }

        for (int i = 0; i <= 8; i += 3)
        {
            _grid.AddRow(panels[i], panels[i + 1], panels[i + 2]);
        }

        var panel = new Panel(_grid).Header("Units").Expand();
        _layout["Battle"]["Turn"]["Units"].Update(panel);
        _displayContext.Refresh();
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