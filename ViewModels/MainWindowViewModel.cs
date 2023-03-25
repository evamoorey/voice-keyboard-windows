using System.Diagnostics;
using Grpc.Core;
using Grpc.Net.Client;
using Prism.Mvvm;
using VoiceKeyboard.Models;

namespace VoiceKeyboard.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private TestModel testModel;

    public MainWindowViewModel()
    {
        Trace.WriteLine("creating client");

        using var channel = GrpcChannel.ForAddress("http://localhost:50033");
        var client = new Commands.CommandsClient(channel);
        try
        {
            var reply = client.AddCommand(
                new AddCommandRequest { Command = "включи штуку", Hotkey = "ctrl+l" });
        }
        catch (RpcException ex)
        {
            Trace.WriteLine(ex.StatusCode);
            Trace.WriteLine(ex.Status.Detail);
        }

        Debug.WriteLine("ha ha ho");

        testModel = new TestModel();
        testModel.Title = "This Is Prism Example";
    }

    public TestModel TestModel
    {
        get => testModel;
        set => SetProperty(ref testModel, value);
    }
}