using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using SpravochnikPP.Context;
using SpravochnikPP.Models;

namespace SpravochnikPP;

public partial class AddProduct : Window
{
    private readonly JvtufokyContext _context;
    private static int _category;
    private static string  NamePath ="";
    public AddProduct()
    {
        InitializeComponent();
        _context = new JvtufokyContext();
        Title = this.Find<TextBox>("Title");
        Calories = this.Find<TextBox>("Calories");
        Carbohydrates = this.Find<TextBox>("Carbohydrates");
        Proteins = this.Find<TextBox>("Proteins");
        Fats = this.Find<TextBox>("Fats");
        GlikIndex = this.Find<TextBox>("GlikIndex");
        CategoryCombobox = this.Find<ComboBox>("CategoryCombobox");
        PhotoButton = this.Find<Button>("PhotoButton");
        Image = this.Find<Image>("Image");
        Exit = this.Find<Button>("Exit");
        PhotoButton = this.Find<Button>("PhotoButton");
        Add = this.Find<Button>("Add");
        Errors = this.Find<TextBlock>("Errors");
        LoadCategories();
    }

    private void LoadCategories()
    {
        try
        {
            List<string> type = new List<string>();
            var a = _context.Categories.ToList();
            type.Add("Выберите категорию");
            foreach (var category in a)
            {
                type.Add(category.Title);
            }
            CategoryCombobox.Items = type;
            CategoryCombobox.SelectedIndex = 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
         try
         {
             _category = CategoryCombobox.SelectedIndex;
             var a2 = _category;
            if (CheckData())
            {
                var a = _context.Kbzus.Add(new Kbzu()
                {
                    Title = Title.Text,
                    Carbohydrates = float.Parse(Carbohydrates.Text),
                    Cal = Int32.Parse(Calories.Text),
                    Proteins = double.Parse(Proteins.Text),
                    Fats = double.Parse(Fats.Text),
                    Categoriesid = a2,
                    Glikindex = Int32.Parse(GlikIndex.Text),
                    Image = NamePath
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
    private bool CheckData()
    {
        Errors.Text = "";
        
        if (string.IsNullOrEmpty(Title.Text)
            || string.IsNullOrEmpty(Calories.Text)
            || string.IsNullOrEmpty(Proteins.Text)
            || string.IsNullOrEmpty(Fats.Text)
            || string.IsNullOrEmpty(Carbohydrates.Text)
            || string.IsNullOrEmpty(GlikIndex.Text)
            || CategoryCombobox.SelectedIndex == 0)
        {
            Errors.Text += "Заполните все поля! ";
            return false;
        }

        if (Title.Text.Length > 40)
        {
            Errors.Text = "Полe названия не больше 40 символов!";
            return false;
        }

        if (IsNumberContains(Title.Text))
        {
            Errors.Text += "Название не может содержать цифры";
            return false;
        }

        if (CategoryCombobox.SelectedIndex == 0)
        {
            Errors.Text += "Выберите категорию!";
            return false;
        }


        foreach (char c in Calories.Text) 
        if (!((Char.IsNumber(c))))
            {
                Errors.Text += "Поле калории содержит недопустивые символы! Пример: 60";
                return false;
            }
        
        foreach (char c in GlikIndex.Text)
            if (!((Char.IsNumber(c))))
            {
                Errors.Text += "Поле гликемического индекса содержит недопустивые символы! Пример: 60";
                return false;
            }
        foreach (char c in Proteins.Text)
            if (!((Char.IsNumber(c)) || c == ','))
            {
                Errors.Text += "Поле белки содержит недопустивые символы! Пример: 6,5";
                return false;
            }
        foreach (char c in Fats.Text)
            if (!((Char.IsNumber(c)) || c == ','))
            {
                Errors.Text += "Поле жиры содержит недопустивые символы! Пример: 6,5";
                return false;
            }
        foreach (char c in Carbohydrates.Text)
            if (!((Char.IsNumber(c)) || c == ','))
            {
                Errors.Text += "Поле углеводы содержит недопустивые символы! Пример: 6,5";
                return false;
            }

        
        
        if (Errors.Text.Length == 0)
            return true;
        return false;
    }

    static bool IsNumberContains(string input)
    {
        foreach (char c in input)
            if (Char.IsNumber(c))
                return true;
        return false;
    }
    private void CategoryCombobox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        CheckData();
    }

    private async void PhotoButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var image = Path.Combine(Directory.GetCurrentDirectory(), "Images").Replace("\\bin\\Debug\\net7.0", "");
            var open = new OpenFileDialog();
            open.AllowMultiple = false;
            open.Title = "Выбери фото";
            open.Filters.Add( new FileDialogFilter() {Name = "Images", Extensions = {"jpg", "png", "gif","jpeg"}});
            var select = await open.ShowAsync((Window)this.VisualRoot);
            if (select != null && select.Length > 0)
            {
                var imagePath = select[0];
                var bitmap = new Bitmap(imagePath);
                Image.Source = bitmap;
                var fileName = Path.GetFileName(imagePath);
                var path = Path.Combine(image, fileName);
                File.Copy(imagePath, path, true);
                NamePath = fileName;
            }
            else
            {
                NamePath = "icon.png";
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
        
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}