using Prism.Mvvm;

namespace VoiceKeyboard.Models;

public class TestModel : BindableBase
{
    private string title;

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }
}