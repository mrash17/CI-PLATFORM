using System;
using System.Collections.Generic;

namespace ciplatform.entities.Models;

public partial class Comment
{
    public long CommentId { get; set; }

    public long UserId { get; set; }

    public long MissionId { get; set; }

    public bool ApprovalStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string ComentDescription { get; set; } = null!;

    public virtual Mission Mission { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
