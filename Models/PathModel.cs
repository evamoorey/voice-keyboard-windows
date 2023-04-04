using GalaSoft.MvvmLight;

namespace VoiceKeyboard.Models;

public class PathModel : ObservableObject
{
    public PathModel(string path)
    {
        Path = path;
    }

    private string path;

    public string Path
    {
        get => path;
        set => Set(() => Path, ref path, value);
    }
}