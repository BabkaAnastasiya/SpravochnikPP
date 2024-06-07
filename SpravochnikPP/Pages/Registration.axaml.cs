using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpravochnikPP.Context;
using SpravochnikPP.Models;

namespace SpravochnikPP.Pages;

public partial class Registration : UserControl
{
    private readonly JvtufokyContext _context;

    public Registration()
    {
        InitializeComponent();
        _context = new JvtufokyContext();
        FirstNameTextBox = this.Find<TextBox>("FirstNameTextBox");
        NameTextBox = this.Find<TextBox>("NameTextBox");
        LoginTextBox = this.Find<TextBox>("LoginTextBox");
        PasswordTextBox = this.Find<TextBox>("PasswordTextBox");
        PatronomicTextBox = this.Find<TextBox>("PatronomicTextBox");
        Errors = this.Find<TextBlock>("Errors");
    }

    private bool CheckData()
    {
        Errors.Text = "";
        if (string.IsNullOrEmpty(FirstNameTextBox.Text)
            || string.IsNullOrEmpty(NameTextBox.Text)
            || string.IsNullOrEmpty(PasswordTextBox.Text)
            || string.IsNullOrEmpty(PatronomicTextBox.Text)
            || string.IsNullOrEmpty(LoginTextBox.Text))
            {
                Errors.Text += "Заполните все поля! ";
                return false;
            }

            if (NameTextBox.Text.Length > 15
                && PatronomicTextBox.Text.Length > 15
                && FirstNameTextBox.Text.Length > 15
                && PasswordTextBox.Text.Length > 15
                && LoginTextBox.Text.Length > 15)
            {
                Errors.Text = "Поля имени, фамилии, отчества, логина и пароля не больше 15 символов!";
                return false;
            }

            if (IsNumberContains(NameTextBox.Text))
            {
                Errors.Text += "Имя не может содержать цифры";
                return false;
            }

            if (IsNumberContains(FirstNameTextBox.Text))
            {
                Errors.Text += "Фамилия не может содержать цифры";
                return false;
            }

            if (IsNumberContains(PatronomicTextBox.Text))
            {
                Errors.Text += "Отчество не может содержать цифры";
                return false;
            }

            if (IsLoginExists(LoginTextBox.Text))
            {
                Errors.Text += "Логин уже существует";
                return false;
            }

            if (Errors.Text.Length == 0)
        {return true;}
        else
        { return false; }
    }
    
    private bool IsLoginExists(string login)
    {
            try
            {
                // Используем _context для проверки существования логина
                bool loginExists = _context.Users.Any(u => u.Login == login);
                return loginExists;
            }
            catch (Exception ex)
            {
                Errors.Text = $"Ошибка при проверке логина: {ex.Message}";
                return true; // Если произошла ошибка при проверке, лучше вернуть true, чтобы предотвратить регистрацию с возможным повторяющимся логином
            }
    }

    static bool IsNumberContains(string input)
    {
        foreach (char c in input)
            if (Char.IsNumber(c))
                return true;
        return false;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (CheckData())
            {
                var a = _context.Users.Add(new User()
                {
                    Firstname = FirstNameTextBox.Text,
                    Lastname = NameTextBox.Text,
                    Patronomic = PatronomicTextBox.Text,
                    Login = LoginTextBox.Text,
                    Passwords =  PasswordTextBox.Text,
                    Roleid = 2,
                });
                _context.SaveChanges();
                Errors.Text = "Сохранено";
            }
        }
        catch (Exception exception)
        {
           
            Console.WriteLine(exception);
           
        }
    }

    private void ExitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Autorization autorization = new Autorization();
        NavigationManager.NavigateTo(autorization);
    }
}