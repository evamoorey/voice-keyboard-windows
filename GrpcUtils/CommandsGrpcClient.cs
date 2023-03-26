using System.Windows;

namespace VoiceKeyboard.GrpcUtils;

public class CommandsGrpcClient: GrpcClient
{
    private readonly Commands.CommandsClient client;

    private static CommandsGrpcClient? instance;

    private CommandsGrpcClient(): base(GrpcClientUtil.ServerHost, GrpcClientUtil.ServerPort)
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
}