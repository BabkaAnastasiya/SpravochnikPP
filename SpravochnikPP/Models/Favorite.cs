using System;
using System.Collections.Generic;

namespace SpravochnikPP.Models;

public partial class Favorite
{
    public int Id { get; set; }

    public int Productid { get; set; }

    public int Userid { get; set; }

    public virtual Kbzu Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
