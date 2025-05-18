public class Menu
{
    private string _name;
    private string[] _selections;
    public ICommand[] _commands;

    public static Menu Create(string name, string[] selections, ICommand[] commands)
    {
        return new Menu(name, selections, commands);
    }

    public Menu(string name, string[] selections, ICommand[] commands)
    {
        _name = name;

        _selections = new string[selections.Length];
        Array.Copy(selections, _selections, selections.Length);

        _commands = new ICommand[commands.Length];
        Array.Copy(commands, _commands, commands.Length);
    }

    public int GetInput()
    {
        string input = Console.ReadLine();
        if (int.TryParse(input, out int choose) && choose >= 1 && choose <= _selections.Length)
        {
            Console.WriteLine();
            return choose;
        }

        Printer.Print("Некорректный выбор.", ConsoleColor.White);
        return GetInput();
    }

    public void Select(int index)
    {
        _commands[index - 1].Execute();
    }

    public void Show()
    {
        Printer.Print($"====={_name}=====", ConsoleColor.White);
        for (int i = 0; i < _selections.Length; i++)
        {
            Printer.Print($"{i+1}) {_selections[i]}", ConsoleColor.White);
        }
    }
}
