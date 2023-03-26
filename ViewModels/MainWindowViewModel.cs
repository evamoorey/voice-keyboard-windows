using System.Collections.ObjectModel;
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
    private ObservableCollection<CommandModel> commands;

    public CommandModel CommandModel
    {
        get => commandModel;
        set
        {
            commandModel = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<CommandModel> CommandsList
    {
        get => commands;
        set
        {
            commands = value;
            RaisePropertyChanged();
        }
    }


    public MainWindowViewModel()
    {
        commandsClient = CommandsGrpcClient.GetInstance();
        appControlClient = AppControlGrpcClient.GetInstance();
        CreateModels();
        CreateCommands();
        UpdateCommandsList();
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
        {
            commandsClient.AddCommand(CommandModel.Command, CommandModel.Hotkey);
            UpdateCommandsList();
        });
        DeleteCommandCommand = new RelayCommand(() =>
        {
            commandsClient.DeleteCommand(CommandModel.Command);
            UpdateCommandsList();
        });
    }

    private void UpdateCommandsList()
    {
        var t = commandsClient.GetCommands();
        CommandsList = new ObservableCollection<CommandModel>();
        foreach (var pair in t)
        {
            CommandsList.Add(new CommandModel(pair.Key, pair.Value));
        }

    }
}