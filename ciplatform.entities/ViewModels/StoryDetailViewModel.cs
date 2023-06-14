using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class StoryDetailViewModel
    {

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }
        public string? Avatar { get; set; }

        public string MissionTitle { get; set; } = null!;

        public string StoryTitle { get; set; } = null!;

        public string? Description { get; set; }

        public string? WhyIVolunteer { get; set; }
        public long MissionId { get; set; }
        public List<StoryMedium> Media { get; set; } = new List<StoryMedium> { };

        public long StoryId { get; set; }
        public long Viewcount { get; set; }


    }
}
