using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.Models;

namespace VoiceKeyboard.ViewModels;

public class MainWindowViewModel : ViewModelBase
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


    public ICommand AddCommandCommand { get; private set; }
    public ICommand DeleteCommandCommand { get; private set; }

    private void CreateCommands()
    {
        AddCommandCommand = new RelayCommand(() =>
            commandsClient.AddCommand(CommandModel.Command, CommandModel.Hotkey));
        DeleteCommandCommand = new RelayCommand(() => commandsClient.DeleteCommand(CommandModel.Command));
        
    }
}