using Prism.Mvvm;

namespace VoiceKeyboard.Models;

public class CommandModel : BindableBase
{
    public CommandModel(string command, string hotkey)
    {
        Command = command;
        Hotkey = hotkey;
    }

    private string command;

    public string Command
    {
        get => command;
        set => SetProperty(ref command, value);
    }

    private string hotkey;

    public string Hotkey
    {
        get => hotkey;
        set => SetProperty(ref hotkey, value);
    }
}