using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class MissionApplicationForAdminViewModel
    {
        public long MissionApplicationId { get; set; }

        public long MissionId { get; set; }

        public long UserId { get; set; }
        public string MissionTitle { get; set; } = null!;
        public string UserName { get; set; } = null!;


        public DateTime AppliedAt { get; set; }

        public string ApprovalStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
