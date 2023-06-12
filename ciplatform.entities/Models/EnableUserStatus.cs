using System;
using System.Collections.Generic;

namespace ciplatform.entities.Models;

public partial class EnableUserStatus
{
    public long Enableuserid { get; set; }

    public long? NotificationId { get; set; }

    public long? UserId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual NotificationTitle? Notification { get; set; }

    public virtual User? User { get; set; }
}
