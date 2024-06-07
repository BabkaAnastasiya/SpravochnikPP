using Avalonia.Controls;
using SpravochnikPP.Pages;

namespace SpravochnikPP;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        contentControl = this.FindControl<ContentControl>("contentControl");
        NavigationManager.Initialize(contentControl);
        ShowMainPage();
    }
    private void ShowMainPage()
    {
        contentControl.Content = new Autorization();
    }
}