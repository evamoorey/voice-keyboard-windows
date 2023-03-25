using Prism.Commands;
using Prism.Mvvm;

namespace VoiceKeyboard.ViewModels;

public partial class MainWindowViewModel
{
    public DelegateCommand AddCommandCommand =>
        addCommandCommand ??= new DelegateCommand(ExecuteAddCommandCommand);

    private void ExecuteAddCommandCommand()
    {
        commandsClient.AddCommand(commandModel.Command, commandModel.Hotkey);
    }
}