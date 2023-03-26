using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.Models;

namespace VoiceKeyboard.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly CommandsGrpcClient commandsClient;
    private readonly AppControlGrpcClient appControlClient;

    private CommandModel commandModel;

    public CommandModel CommandModel
    {
        get => commandModel;
        set
        {
            commandModel = value;
            RaisePropertyChanged();
        }
    }


    public MainWindowViewModel()
    {
        commandsClient = CommandsGrpcClient.GetInstance();
        appControlClient = AppControlGrpcClient.GetInstance();
        CreateModels();
        CreateCommands();
    }

    private void CreateModels()
    {
        CommandModel = new CommandModel("Команда", "Хоткей");
    }

    private void CreateCommands()
    {
        AddCommandCommand = new RelayCommand(AddCommand);
        DeleteCommandCommand = new RelayCommand(DeleteCommand);
    }
}