using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpravochnikPP.Context;
using SpravochnikPP.Pages;

namespace SpravochnikPP;

public partial class Autorization : UserControl
{
    private readonly JvtufokyContext _context;
    public Autorization()
    {
        InitializeComponent();
        _context = new JvtufokyContext();
        LoginTextBox = this.Find<TextBox>("LoginTextBox");
        PasswordTextBox = this.Find<TextBox>("PasswordTextBox");
        EnterButton = this.Find<Button>("EnterButton");
        RegistrationButton = this.Find<Button>("RegistrationButton");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void EnterButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            var user = _context.Users.Where(x => x.Login == login && x.Passwords == password).FirstOrDefault();
            if (user != null)
            {
                MainPage mainPage = new MainPage(user);
                NavigationManager.NavigateTo(mainPage);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
        

    }

    private void RegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Registration registration = new Registration();
        NavigationManager.NavigateTo(registration);

    }
}