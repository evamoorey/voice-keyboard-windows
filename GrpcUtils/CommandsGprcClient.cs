using System;
using System.Diagnostics;
using System.Windows;
using Grpc.Core;
using Grpc.Net.Client;

namespace VoiceKeyboard.GrpcUtils;

public class CommandsGprcClient
{
    private Commands.CommandsClient client;

    private static CommandsGprcClient? instance;

    private CommandsGprcClient()
    {
        var channel = GrpcChannel.ForAddress($"http://{GrpcClientUtil.ServerHost}:{GrpcClientUtil.ServerPort}");
        client = new Commands.CommandsClient(channel);
    }

    public static CommandsGprcClient GetInstance()
    {
        return instance ??= new CommandsGprcClient();
    }

    private void TryMakeRequest(Action action)
    {
        try
        {
            action();
        }
        catch (RpcException ex)
        {
            MessageBox.Show(ex.Status.StatusCode == StatusCode.Unavailable
                ? "Ошибка локального сервера"
                : ex.Status.Detail);
        }
    }

    public void AddCommand(string command, string hotkey)
    {
        void Action()
        {
            client.AddCommand(new AddCommandRequest { Command = command, Hotkey = hotkey });

            MessageBox.Show("Команда добавлена");
        }

        TryMakeRequest(Action);
    }
}