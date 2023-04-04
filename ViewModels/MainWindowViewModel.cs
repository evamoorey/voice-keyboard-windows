using System.Collections.ObjectModel;
using System.IO;
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
    private AppStateModel appStateModel;
    private PathModel pathModel;
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

    public AppStateModel AppStateModel
    {
        get => appStateModel;
        set
        {
            appStateModel = value;
            RaisePropertyChanged();
        }
    }

    public PathModel PathModel
    {
        get => pathModel;
        set
        {
            pathModel = value;
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
        PathModel = new PathModel("Путь для импорта/экспорта");
        AppStateModel = new AppStateModel(true);
    }


    public ICommand AddCommandCommand { get; private set; }
    public ICommand DeleteCommandCommand { get; private set; }
    public ICommand ChangeMicrophoneStatusCommand { get; private set; }
    public ICommand ImportCommandsCommand  { get; private set; }
    public ICommand ExportCommandsCommand  { get; private set; }


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

        ChangeMicrophoneStatusCommand = new RelayCommand(
            () => appControlClient.ChangeMicrophoneStatus(AppStateModel.IsMicrophoneOn));

        ImportCommandsCommand = new RelayCommand(() =>
        {
            commandsClient.ImportCommands(PathModel.Path);
            UpdateCommandsList();
        });
        ExportCommandsCommand  = new RelayCommand(
            () => commandsClient.ExportCommands(PathModel.Path));
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