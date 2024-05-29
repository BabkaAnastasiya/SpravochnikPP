using Avalonia.Controls;

namespace SpravochnikPP;


public class NavigationManager
{
    private static ContentControl contentControl;

    public static void Initialize(ContentControl control)
    {
        contentControl = control;
    }

    public static void NavigateTo(UserControl page)
    {
        contentControl.Content = page;
    }
}