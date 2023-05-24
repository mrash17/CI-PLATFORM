using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class StoryPageForAdminViewModel
    {
        public long MissionId { get; set; }

        public long StoryId { get; set; }

        public string MissionTitle { get; set; } = null!;
        public string StoryTitle { get; set; } = null!;

        public string UserName { get; set; } = null!;


        public int Status { get; set; }

        
    }
}
