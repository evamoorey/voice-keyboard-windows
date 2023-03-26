using System.Collections.Generic;
using System.Windows;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VoiceKeyboard.GrpcUtils;

public class CommandsGrpcClient : GrpcClient
{
    private readonly Commands.CommandsClient client;

    private static CommandsGrpcClient? instance;

    private CommandsGrpcClient() : base(GrpcClientUtil.ServerHost, GrpcClientUtil.ServerPort)
    {
        client = new Commands.CommandsClient(Channel);
    }

    public static CommandsGrpcClient GetInstance()
    {
        return instance ??= new CommandsGrpcClient();
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

    public void DeleteCommand(string command)
    {
        void Action()
        {
            client.DeleteCommand(new DeleteCommandRequest { Command = command });

            MessageBox.Show("Команда удалена");
        }

        TryMakeRequest(Action);
    }

    public IDictionary<string, string> GetCommands()
    {
        try
        {
            GetCommandsResponse resp = client.GetCommands(new Empty());
            return resp.Commands;
        }
        catch (RpcException ex)
        {
            MessageBox.Show(ex.Status.StatusCode == StatusCode.Unavailable
                ? "Ошибка локального сервера"
                : ex.Status.Detail);
            return new Dictionary<string, string>();
        }
    }
}