using System;
using System.Collections.Generic;

namespace SpravochnikPP.Models;

public partial class Kbzu
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Glikindex { get; set; }

    public double? Cal { get; set; }

    public double? Proteins { get; set; }

    public double? Fats { get; set; }

    public double? Carbohydrates { get; set; }

    public int? Categoriesid { get; set; }

    public string? Image { get; set; }

    public virtual Category? Categories { get; set; }
}
