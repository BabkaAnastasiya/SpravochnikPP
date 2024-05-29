using System;
using System.Collections.Generic;

namespace SpravochnikPP.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Kbzu> Kbzus { get; set; } = new List<Kbzu>();
}
