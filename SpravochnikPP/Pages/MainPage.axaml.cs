using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Color = Avalonia.Media.Color;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SpravochnikPP.Context;
using SpravochnikPP.Models;
using TextBox = Avalonia.Controls.TextBox;

namespace SpravochnikPP.Pages;

public partial class MainPage : UserControl
{
    private readonly JvtufokyContext _context;
    private User user;
    public MainPage()
    {
        InitializeComponent();
        _context = new JvtufokyContext();
        SearchTextBox = this.Find<TextBox>("SearchTextBox");
        FiltrComboBox = this.Find<ComboBox>("FiltrComboBox");
        SortComboBox = this.Find<ComboBox>("SortComboBox");
        ListBox = this.Find<ListBox>("ListBox");
        TextDocument = this.Find<TextBlock>("TextDocument");
        DocxDocument = this.Find<TextBlock>("DocxDocument");
        CountTextBlock = this.Find<TextBlock>("CountTextBlock");
        KalculButton = this.Find<Button>("KalculButton");
        ResultTextBlock = this.Find<TextBlock>("ResultTextBlock");
        HeightComboBox = this.Find<ComboBox>("HeightComboBox");
        WeightComboBox = this.Find<ComboBox>("WeightComboBox");
        
        LoadDocxDocument(@"..\..\..\Files\Текстовый документ.txt");
        LoadTextDocument(@"..\..\..\Files\IMT.txt");
        LoadHeight();
        LoadWeight();
        LoadCategories(); 
        FillList();
    }

    public MainPage(User user)
    {
        InitializeComponent();
        this.user = user;
        _context = new JvtufokyContext();
        SearchTextBox = this.Find<TextBox>("SearchTextBox");
        FiltrComboBox = this.Find<ComboBox>("FiltrComboBox");
        SortComboBox = this.Find<ComboBox>("SortComboBox");
        ListBox = this.Find<ListBox>("ListBox");
        TextDocument = this.Find<TextBlock>("TextDocument");
        DocxDocument = this.Find<TextBlock>("DocxDocument");
        CountTextBlock = this.Find<TextBlock>("CountTextBlock");
        KalculButton = this.Find<Button>("KalculButton");
        ResultTextBlock = this.Find<TextBlock>("ResultTextBlock");
        HeightComboBox = this.Find<ComboBox>("HeightComboBox");
        WeightComboBox = this.Find<ComboBox>("WeightComboBox");
        AddButton = this.Find<Button>("AddButton");
        LoadDocxDocument(@"..\..\..\Files\Текстовый документ.txt");
        LoadTextDocument(@"..\..\..\Files\IMT.txt");
        LoadHeight();
        LoadWeight();
        LoadCategories(); 
        FillList();
        if (this.user.Roleid == 1)
        {
            AddButton.IsVisible = true;
        }
        else
        {
            AddButton.IsVisible = false;
        }
    }

    private void LoadCategories()
    {
        try
        {
            List<string> type = new List<string>();
            var a = _context.Categories.ToList();
            type.Add("Категории");
       
            foreach (var category in a)
            {
                type.Add(category.Title);
            }
        
            FiltrComboBox.Items = type;
            FiltrComboBox.SelectedIndex = 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private void LoadHeight()
    {
        List<int> height = new List<int>();
        for (int i = 100; i <= 250; i++)
        {
            height.Add(i);
        }
        
        HeightComboBox.Items = height;
    }
    
    private void LoadWeight()
    {
        List<int> weight = new List<int>();
        for (int i = 30; i <= 300; i++)
        {
            weight.Add(i);
        }
        
        WeightComboBox.Items = weight;
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void FillList()
    {
        List<Kbzu> kbzuList = _context.Kbzus.ToList();
        if (!string.IsNullOrEmpty(SearchTextBox.Text))
        {
            string[] searchwords = SearchTextBox.Text.ToLower()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            kbzuList = kbzuList.Where(a => searchwords.All(word =>
                (a.Title != null && a.Title.ToLower().Contains(word)))).ToList();
        }
        if (FiltrComboBox != null && FiltrComboBox.SelectedItem != null && FiltrComboBox.SelectedIndex != 0)
        {
            kbzuList = kbzuList.Where(a => a.Categories.Id == FiltrComboBox.SelectedIndex).ToList();
        }
        if (SortComboBox != null && SortComboBox.SelectedIndex == 1)
        {
            kbzuList = kbzuList.OrderBy(a => a.Title).ToList();
        }
        if (SortComboBox != null && SortComboBox.SelectedIndex == 2)
        {
            kbzuList = kbzuList.OrderByDescending(a => a.Title).ToList();
        }
        if (SortComboBox != null && SortComboBox.SelectedIndex == 3)
        {
            kbzuList = kbzuList.OrderBy(a => a.Glikindex).ToList();
        }
        if (SortComboBox != null && SortComboBox.SelectedIndex == 4)
        {
            kbzuList = kbzuList.OrderByDescending(a => a.Glikindex).ToList();
        }
        
        List<KBZUCard> kbzu = new List<KBZUCard>();
        foreach (var VARIABLE in kbzuList)
        {
            KBZUCard kbzuCard = new KBZUCard();
            kbzuCard.Image = GetBitmap(VARIABLE.Image);
            kbzuCard.Title = VARIABLE.Title;
            kbzuCard.Cal = VARIABLE.Cal;
            kbzuCard.Carbohydrates = VARIABLE.Carbohydrates;
            kbzuCard.Glikindex = VARIABLE.Glikindex;
            kbzuCard.Proteins = VARIABLE.Proteins;
            kbzuCard.Fats = VARIABLE.Fats;
            kbzuCard.Id = VARIABLE.Id;
            if (VARIABLE.Glikindex <= 20)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(189,236,182));
            }
            else if (VARIABLE.Glikindex > 20 && VARIABLE.Glikindex <=40)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(116, 209,69));
            }
            else if (VARIABLE.Glikindex > 40 && VARIABLE.Glikindex <= 69)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(232,208,86));
            }
            else if (VARIABLE.Glikindex > 69 )
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(255,166,162));
            }
            kbzu.Add(kbzuCard);
        }
        ListBox.Items = kbzu;
        
        CountTextBlock.Text = kbzu.Count + " из " + _context.Kbzus.Count();
    }
    
    private Bitmap GetBitmap(string Photopath)
    {
        try
        {
            var img = @"..\..\..\Images\";
            var image = new Bitmap(img + Photopath);
            return image;

        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    private void SearchTextBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        FillList();
    }
    private void SortComboBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        FillList();
    }
    private void FiltrComboBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
       FillList();
    }

    private void LoadDocxDocument(string filePath)
    {
        try
        {
            // Чтение текстового файла
            string text = File.ReadAllText(filePath);
            // Установка текста в TextBox
            DocxDocument.Text = text;
        }
        catch (Exception ex)
        {
            // Обработка ошибок, если файл не может быть прочитан или не существует
            Console.WriteLine("Error loading text document: " + ex.Message);
        }

    }
    private void LoadTextDocument(string filePath)
    {
        try
        {
            // Чтение текстового файла
            string text = File.ReadAllText(filePath);
            // Установка текста в TextBox
            TextDocument.Text = text;
        }
        catch (Exception ex)
        {
            // Обработка ошибок, если файл не может быть прочитан или не существует
            Console.WriteLine("Error loading text document: " + ex.Message);
        }

    }


    private void KalculButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (WeightComboBox.SelectedIndex != 0 && HeightComboBox.SelectedIndex != 0)
            {
                double rost = double.Parse(HeightComboBox.SelectedItem.ToString()) / 100;
                double ves = Int32.Parse(WeightComboBox.SelectedItem.ToString());
        
                double imt = ves / Math.Pow(rost,2);
                ResultTextBlock.Text = imt.ToString();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
    }


    private void HeightComboBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        
    }

    private void WeightComboBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AddProduct addProduct = new AddProduct();
        addProduct.Show();
    }

    private void AddRacionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var selectedProduct = (sender as Button).Tag.ToString();
        try
        {
            var a = _context.Favorites.Add(new Favorite()
            {
                Productid = Int32.Parse(selectedProduct),
                Userid = this.user.Id
            });
            _context.SaveChanges();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
    }

    private void RacionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var user = _context.Users.Where(x => x.Id == this.user.Id).FirstOrDefault();
            if (user != null)
            {
                Racion racion = new Racion(user);
                NavigationManager.NavigateTo(racion);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
    }
}