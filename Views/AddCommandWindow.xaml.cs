using System.Windows;
using VoiceKeyboard.ViewModels;

namespace VoiceKeyboard.Views;

public partial class AddCommandWindow : Window
{
    public AddCommandWindow()
    {
        InitializeComponent();
        DataContext = new AddCommandViewModel();
        
    }
}