using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.Models;
using VoiceKeyboard.ViewModels;
using Forms = System.Windows.Forms;
using Application = System.Windows.Application;

namespace VoiceKeyboard.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Forms.NotifyIcon? icon;
    private MainWindowViewModel mainWindowViewModel;

    public MainWindow()
    {
        InitializeComponent();
        StartServer();
        CreateIcon();

        mainWindowViewModel = new MainWindowViewModel();
        DataContext = mainWindowViewModel;

        Closed += OnShutdown;
    }

    private void OnShutdown(object? sender, EventArgs e)
    {
        ShutdownServer();
        DisposeIcon();
    }

    private void StartServer()
    {
        Trace.WriteLine("starting server");

        var p = new Process();

        var info =
            new ProcessStartInfo("Server/vk_server.exe");
        info.Arguments = "-p windows";
        info.RedirectStandardOutput = false;
        info.RedirectStandardInput = false;
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        p.StartInfo = info;

        p.Start();

        WaitServerForStart();
    }

    private void CreateIcon()
    {
        Forms.NotifyIcon icon = new Forms.NotifyIcon();
        var rs = Application
            .GetResourceStream(new Uri("pack://application:,,,/VoiceKeyboard;component/Resources/logo.ico"));
        if (rs == null)
        {
            return;
        }

        Stream iconStream = rs.Stream;
        icon.Icon = new System.Drawing.Icon(iconStream);
        icon.Visible = true;
        icon.Text = "Voice Keyboard";
        

        icon.ContextMenuStrip = new Forms.ContextMenuStrip();
        icon.ContextMenuStrip.Items.Add("Добавить команду", null, (_, _) => new AddCommandWindow().ShowDialog());
        icon.ContextMenuStrip.Items.Add("Введённые команды", null, (_, _) => new CommandsWindow().ShowDialog());
        icon.ContextMenuStrip.Items.Add("Импорт команд", null,
            (_, _) => mainWindowViewModel.ImportCommandsCommand.Execute(null));
        icon.ContextMenuStrip.Items.Add("Экспорт команд", null,
            (_, _) => mainWindowViewModel.ExportCommandsCommand.Execute(null));
        icon.ContextMenuStrip.Items.Add("Распознавание команд", null, (_, _) =>
        {
            if (((Forms.ToolStripMenuItem)icon.ContextMenuStrip.Items[4]).Checked)
            {
                mainWindowViewModel.ChangeMicrophoneStatusCommand.Execute(false);
                ((Forms.ToolStripMenuItem)icon.ContextMenuStrip.Items[4]).Checked = false;
            }
            else
            {
                mainWindowViewModel.ChangeMicrophoneStatusCommand.Execute(true);
                ((Forms.ToolStripMenuItem)icon.ContextMenuStrip.Items[4]).Checked = true;
            }
        });
        icon.ContextMenuStrip.Items.Add("Закрыть", null, (_, _) => Close());

        ((Forms.ToolStripMenuItem)icon.ContextMenuStrip.Items[4]).Checked = true;

        this.icon = icon;
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            Hide();
        }

        base.OnStateChanged(e);
    }

    private void DisposeIcon()
    {
        icon?.Dispose();
    }

    private void WaitServerForStart()
    {
        int errPingCount = 0;
        while (!GrpcClientUtil.PingServer(GrpcClientUtil.ServerHost, GrpcClientUtil.ServerPort))
        {
            errPingCount++;
            if (errPingCount >= 5)
            {
                MessageBox.Show("Невозможно запустить локальный сервер приложения");
                Environment.Exit(-1);
            }
        }

        Thread.Sleep(1000);
    }

    private void ShutdownServer()
    {
        Trace.WriteLine("stopping server");
        foreach (var p in Process.GetProcessesByName("vk_server"))
        {
            p.Kill();
            p.WaitForExit();
        }
    }
}