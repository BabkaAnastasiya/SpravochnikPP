using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SpravochnikPP.Context;
using SpravochnikPP.Models;

namespace SpravochnikPP.Pages;

public partial class Racion : UserControl
{
    private User user;
    private readonly JvtufokyContext _context;
    
    public Racion()
    {
        InitializeComponent();
    }
    public Racion(User user)
    {
        InitializeComponent();
        this.user = user;
        _context = new JvtufokyContext();
        ListBox = this.Find<ListBox>("ListBox");
        CalTextBlock = this.Find<TextBlock>("CalTextBlock");
        FillList(user);
    }
    private void FillList(User user)
    {
        List<Favorite> favorites = _context.Favorites.Where(x => x.Userid == user.Id).ToList();
        List<KBZUCard> kbzu = new List<KBZUCard>();

        var cal = 0;
        foreach (var VARIABLE in favorites)
        {
            KBZUCard kbzuCard = new KBZUCard();
            var calories = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Cal;
            kbzuCard.Image = GetBitmap(_context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Image);
            kbzuCard.Title = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Title;
            kbzuCard.Cal = calories;
            kbzuCard.Carbohydrates = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Carbohydrates;
            kbzuCard.Glikindex = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Glikindex;
            kbzuCard.Proteins = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Proteins;
            kbzuCard.Fats = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Fats;
            kbzuCard.Id = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Id;
            var glikIndex = _context.Kbzus.Where(x => x.Id == VARIABLE.Productid).FirstOrDefault().Glikindex;
            if (glikIndex <= 20)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(189,236,182));
            }
            else if (glikIndex > 20 && glikIndex <=40)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(116, 209,69));
            }
            else if (glikIndex > 40 && glikIndex <= 69)
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(232,208,86));
            }
            else if (glikIndex > 69 )
            {
                kbzuCard.Background = new SolidColorBrush(Color.FromRgb(255,166,162));
            }
            kbzu.Add(kbzuCard);
            cal += calories;

        }
        ListBox.Items = kbzu;
        CalTextBlock.Text = $"Калорий: {cal.ToString()}";
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var id = (sender as Button).Tag.ToString();
            var x = _context.Favorites.FirstOrDefault(a => a.Productid == Int32.Parse(id));
            _context.Remove(x);
            _context.SaveChanges();
            FillList(user);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
        }
       
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var user = _context.Users.Where(x => x.Id == this.user.Id).FirstOrDefault();
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
}