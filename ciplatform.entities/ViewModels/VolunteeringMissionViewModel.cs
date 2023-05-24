using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class VolunteeringMissionViewModel
    {
        public List<Mission> RelatedMissions { get; set; } = new List<Mission>();

        public long MissionId { get; set; }

        public List<MissionMedium> missionMediaMediums { get; set; } = new List<MissionMedium> { };
        public string Theme { get; set; } = String.Empty;
        public List<string> SkillList { get; set; } = new List<string>();
        public List<Timesheet> Timesheet { get; set; } 
        public List<GoalMission> goalMissions { get; set; } 

        public string City { get; set; } = String.Empty;

        public long totalSeats { get; set; }


        public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? deadlineDate { get; set; }

        public bool MissionType { get; set; }

        public bool? Status { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public List<MissionRating> MissionRatings { get; set; } = new List<MissionRating>();
        public bool favouritMissions { get; set; }


        public List<Comment> ViewComments { get; set; } = new List<Comment> { };
        public List<MissionApplication> Missionapplications { get; set; } = new List<MissionApplication> { };

        public List<MissionDocument> missionDocuments { get; set; } = new List<MissionDocument> { };



    }
}
