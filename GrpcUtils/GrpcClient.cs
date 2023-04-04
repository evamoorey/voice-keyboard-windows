using System;
using System.Windows;
using Grpc.Core;
using Grpc.Net.Client;

namespace VoiceKeyboard.GrpcUtils;

public abstract class GrpcClient : IDisposable
{
    protected GrpcChannel Channel;

    protected GrpcClient(string addr, int port)
    {
        Channel = GrpcChannel.ForAddress($"http://{addr}:{port}");
    }
    
    protected void TryMakeRequest(Action action)
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

    public void Dispose()
    {
        Channel.Dispose();
    }
}