using System.Windows.Input;

namespace VoiceKeyboard.ViewModels;

public partial class MainWindowViewModel
{
    public ICommand AddCommandCommand { get; private set; }

    private void AddCommand()
    {
        commandsClient.AddCommand(CommandModel.Command, CommandModel.Hotkey);
    }
}