using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ciplatform.entities.ViewModels
{
    public class LandingViewModel
    {
       public List<Country> GetCountryList { get; set; } = new List<Country>();
       public List<City> GetCityList { get; set; } = new List<City> { };
        public List<Skill> GetSkillList { get; set; } = new List<Skill> { };
        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill> { };
        public List<Theme> GetThemesList { get; set; } = new List<Theme> { };
       public IPagedList<Mission> GetMissionDetails { get; set; } 
        public int currentPage { get; set; }
        public int totalrecord { get; set; }
        public int id { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string searchkey { get; set; } = string.Empty;
        public List<MissionRating> MissionRatings { get; set; } = new List<MissionRating> { };
        public List<MissionMedium> missionMedia { get; set; } = new List<MissionMedium> { };
        public List<FavouritMission> favouritMissions { get; set; } = new List<FavouritMission> { };

    }
}
