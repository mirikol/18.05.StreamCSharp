public class Menu
{
    public int BindingsCount => _bindings.Length;

    private string _name;
    private CommandBinding[] _bindings;

    public Menu(string name, CommandBinding[] bindings)
    {
        _name = name;

        _bindings = new CommandBinding[bindings.Length];
        Array.Copy(bindings, _bindings, bindings.Length);
    }

    public int GetInput()
    {
        string input = Console.ReadLine();
        if (int.TryParse(input, out int choose) && choose >= 1 && choose <= _bindings.Length)
        {
            Console.WriteLine();
            return choose;
        }

        Printer.Print("Некорректный выбор.", ConsoleColor.White);
        return GetInput();
    }

    public void Select(int index)
    {
        _bindings[index - 1].Command.Execute();
    }

    public void Show()
    {
        Printer.Print($"====={_name}=====", ConsoleColor.White);
        for (int i = 0; i < _bindings.Length; i++)
        {
            Printer.Print($"{i+1}) {_bindings[i].Label}", ConsoleColor.White);
        }
    }
}
