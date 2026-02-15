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

        Panel[] panels = new Panel[15];
        for (int i = 0; i < 15; i++)
        {
            if (i >= 6 && i <= 8)
            {
                panels[i] = new Panel("") { Height = 10 }.BorderColor(Color.Black);
            }
            else
            {
                panels[i] = new Panel("Empty") { Width = 25 }.BorderColor(Color.White);
            }
        }

        for (int i = 0; i < context.UnitsPlacement.Length; i++)
        {
            foreach (var index in context.UnitsPlacement[i])
            {
                panels[GetIndex(index, context.Units[i].IsAlly)] = new Panel(context.Units[i].Unit.Model.Name) { Width = 25 }.BorderColor(Color.White);
            }
        }

        for (int i = 0; i < context.UnitsPlacement.Length; i++)
        {
            if (context.UnitsPlacement[i].Length > 1)
            {
                panels[GetIndex(context.UnitsPlacement[i][0], context.Units[i].IsAlly)] = new Panel("") { Height = 2, Width = 25 }.NoBorder();
                panels[GetIndex(context.UnitsPlacement[i][1], context.Units[i].IsAlly)] = new Panel(context.Units[i].Unit.Model.Name) { Height = 6, Width = 25 };
            }
        }

        for (int i = 0; i <= 12; i += 3)
        {
            _grid.AddRow(panels[i], panels[i + 1], panels[i + 2]);
        }

        //for (int j = 1; j <= 7; j++)
        //{
        //    if (j >= 3 && j <= 5)
        //    {
        //        _grid.AddRow(new Panel("").BorderColor(Color.Black), new Panel("").BorderColor(Color.Black), new Panel("").BorderColor(Color.Black));
        //    }
        //    else
        //    {
        //        _grid.AddRow(new Panel("222").BorderColor(Color.White), new Panel("222").BorderColor(Color.White), new Panel("222").BorderColor(Color.White));
        //    }
        //}

        var panel = new Panel(_grid).Header("Units").Expand();
        _layout["Battle"]["Turn"]["Units"].Update(panel);
        _displayContext.Refresh();
    }

    private int GetIndex(int index, bool isAlly)
    {
        if (isAlly)
        {
            return index + 9;
        }
        else
        {
            int realIndex = index;
            if (realIndex >= 3)
            {
                realIndex -= 3;
            }
            else
            {
                realIndex += 3;
            }

            return realIndex;
        }
    }
}