using Spectre.Console;
using System;

public class UnitsPrinter : IPrinter
{
    private LiveDisplayContext _displayContext;
    private Layout _layout;
    private Grid _grid;

    public void Initialize(LiveDisplayContext context, Layout layout)
    {
        _displayContext = context;
        _layout = layout;
    }

    public void Print(UnitsContext context)
    {
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
                panels[i].AddRow(new Panel("") { Height = 6 }.NoBorder());
                panels[i].AddRow(new Panel("") { Height = 6 }.NoBorder());
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
                            panels[i].AddRow(new Panel(indeciesToName[i][0]) { Height = 6, Width = 25 }.BorderColor(Color.White));
                        }
                        else
                        {
                            panels[i].AddRow(new Panel(indeciesToName[i][0]) { Height = 3, Width = 25 }.BorderColor(Color.White));
                            panels[i].AddRow(new Panel(indeciesToName[i][1]) { Height = 3, Width = 25 }.BorderColor(Color.White));
                        }
                    }
                    else if (!indeciesToName[i].ContainsKey(1))
                    {
                        panels[i].AddRow(new Panel(indeciesToName[i][0]) { Height = 3, Width = 25 }.BorderColor(Color.White));
                        panels[i].AddRow(new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White));
                    }
                    else
                    {
                        panels[i].AddRow(new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White));
                        panels[i].AddRow(new Panel(indeciesToName[i][1]) { Height = 3, Width = 25 }.BorderColor(Color.White));
                    }
                }
                else
                {
                    panels[i].AddRow(new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White));
                    panels[i].AddRow(new Panel("Empty") { Height = 3, Width = 25 }.BorderColor(Color.White));
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