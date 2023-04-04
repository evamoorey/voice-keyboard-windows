using System;

namespace VoiceKeyboard.GrpcUtils;

public class AppControlGrpcClient : GrpcClient
{
    private AppControl.AppControlClient client;

    private static AppControlGrpcClient? instance;

    private AppControlGrpcClient() : base(GrpcClientUtil.ServerHost, GrpcClientUtil.ServerPort)
    {
        client = new AppControl.AppControlClient(Channel);
    }

    public static AppControlGrpcClient GetInstance()
    {
        return instance ??= new AppControlGrpcClient();
    }

    public void ChangeMicrophoneStatus(bool on)
    {
        void Action()
        {
            client.ChangeMicrophoneStatus(new ChangeMicrophoneStatusRequest { On = on });
        }

        TryMakeRequest(Action);
    }
}