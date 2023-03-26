using Prism.Commands;
using Prism.Mvvm;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.Models;

namespace VoiceKeyboard.ViewModels;

public partial class MainWindowViewModel : BindableBase
{
    private CommandModel commandModel;

    private readonly CommandsGrpcClient commandsClient;

    private DelegateCommand? addCommandCommand;


    public MainWindowViewModel()
    {
        commandsClient = CommandsGrpcClient.GetInstance();
        commandModel = new CommandModel("Команда", "Хоткей");
    }

    public CommandModel CommandModel
    {
        get => commandModel;
        set => SetProperty(ref commandModel, value);
    }
}