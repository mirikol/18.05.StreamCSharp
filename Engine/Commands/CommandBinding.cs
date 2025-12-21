public struct CommandBinding
{
    public string Label => _label;
    public ICommand Command => _command;

    private string _label;
    private ICommand _command;

    public CommandBinding(string label, ICommand command)
    {
        _label = label;
        _command = command;
    }
}