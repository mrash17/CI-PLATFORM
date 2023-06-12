using System;
using System.Collections.Generic;

namespace ciplatform.entities.Models;

public partial class ContactU
{
    public long Contactusid { get; set; }

    public long Userid { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
