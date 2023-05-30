﻿using System;
using System.Collections.Generic;

namespace ciplatform.entities.Models;

public partial class MessageTable
{
    public int MessageId { get; set; }

    public long? NotificationId { get; set; }

    public string? Message { get; set; }

    public string? Url { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? AvatarUser { get; set; }

    public virtual NotificationTitle? Notification { get; set; }

    public virtual ICollection<Userpermission> Userpermissions { get; } = new List<Userpermission>();
}
