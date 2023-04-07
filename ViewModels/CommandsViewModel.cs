using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.Models;

namespace VoiceKeyboard.ViewModels;

public class CommandsViewModel : ViewModelBase
{
    private readonly CommandsGrpcClient commandsClient;
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
    
    public CommandsViewModel()
    {
        commandsClient = CommandsGrpcClient.GetInstance();
        CreateModels();
        CreateCommands();
        UpdateCommandsList();
    }
    
    private void CreateModels()
    {
        CommandModel = new CommandModel("", "");
    }
    
    public ICommand DeleteCommandCommand { get; private set; }
    
    private void CreateCommands()
    {
        DeleteCommandCommand = new RelayCommand<CommandModel>(cmd =>
        {
            if (cmd.Command == String.Empty)
            {
                return;
            }
            commandsClient.DeleteCommand(cmd.Command);
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