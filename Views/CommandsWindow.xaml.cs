using System.Windows;
using VoiceKeyboard.ViewModels;

namespace VoiceKeyboard.Views;

public partial class CommandsWindow : Window
{
    public CommandsWindow()
    {
        InitializeComponent();
        DataContext = new CommandsViewModel();
    }
}