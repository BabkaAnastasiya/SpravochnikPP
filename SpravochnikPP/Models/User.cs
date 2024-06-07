using System;
using System.Collections.Generic;

namespace SpravochnikPP.Models;

public partial class User
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronomic { get; set; }

    public string Login { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public int Roleid { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual Role Role { get; set; } = null!;
}
