using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace SpravochnikPP;

public class KBZUCard
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Glikindex { get; set; }

    public double? Cal { get; set; }

    public double? Proteins { get; set; }

    public double? Fats { get; set; }

    public double? Carbohydrates { get; set; }

    public int? Categories { get; set; }

    public Bitmap? Image { get; set; }
    
    public IBrush? Background { get; set; }

}