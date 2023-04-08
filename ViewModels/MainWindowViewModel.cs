using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
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

    public ObservableCollection<CommandModel> CommandsList
    {
        get => commands;
        set
        {
            commands = value;
            RaisePropertyChanged();
        }
    }
    
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
        UpdateCommandsList();
    }
    
    public ICommand ChangeMicrophoneStatusCommand { get; private set; }
    public ICommand ImportCommandsCommand { get; private set; }
    public ICommand ExportCommandsCommand { get; private set; }
    private void CreateModels()
    {
        CommandModel = new CommandModel("", "");
    }

    private void CreateCommands()
    {
        ChangeMicrophoneStatusCommand = new RelayCommand<bool>(
            flag => appControlClient.ChangeMicrophoneStatus(flag));
        ImportCommandsCommand = new RelayCommand(() =>
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".json";
            fileDialog.Filter = "Json files (.json)|*.json";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                commandsClient.ImportCommands(fileDialog.FileName);
                UpdateCommandsList();
            }
        });
        ExportCommandsCommand = new RelayCommand(() =>
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                commandsClient.ExportCommands(folderBrowserDialog.SelectedPath + "\\commands.json");
            }
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