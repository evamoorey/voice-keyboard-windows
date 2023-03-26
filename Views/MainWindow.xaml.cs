using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using VoiceKeyboard.GrpcUtils;
using VoiceKeyboard.ViewModels;

namespace VoiceKeyboard.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        StartServer();

        DataContext = new MainWindowViewModel();

        Closed += OnShutdown;
    }

    private void OnShutdown(object? sender, EventArgs e)
    {
        ShutdownServer();
    }

    private void StartServer()
    {
        Trace.WriteLine("starting server");

        var p = new Process();

        var info =
            new ProcessStartInfo("C:\\Users\\dm1tr\\Desktop\\VoiceKeyboard\\Server\\vk_server.exe");
        info.Arguments = "-p windows -c C:\\Users\\dm1tr\\Desktop\\VoiceKeyboard\\Server\\commands.json";
        info.RedirectStandardOutput = false;
        info.RedirectStandardInput = false;
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        p.StartInfo = info;

        p.Start();

        WaitServerForStart();
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