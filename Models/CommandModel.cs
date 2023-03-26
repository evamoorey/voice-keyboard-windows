using GalaSoft.MvvmLight;

namespace VoiceKeyboard.Models;

public class CommandModel : ObservableObject
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
        set => Set(() => Command, ref command, value);
    }

    private string hotkey;

    public string Hotkey
    {
        get => hotkey;
        set => Set(() => Hotkey, ref hotkey, value);
    }
}