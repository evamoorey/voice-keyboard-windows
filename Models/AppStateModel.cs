using GalaSoft.MvvmLight;

namespace VoiceKeyboard.Models;

public class AppStateModel : ObservableObject
{
    public AppStateModel(bool isMicrophoneOn)
    {
        IsMicrophoneOn = isMicrophoneOn;
    }

    private bool isMicrophoneOn;

    public bool IsMicrophoneOn
    {
        get => isMicrophoneOn;
        set => Set(() => IsMicrophoneOn, ref isMicrophoneOn, value);
    }
}