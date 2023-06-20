using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ciplatform.repository.Repository
{
    [Authorize]
    public class LandingPageRepository : ILandingPageInterface
    {
        private readonly CidbContext _CidbContext;

        public LandingPageRepository(CidbContext cidbContext)
        {
            _CidbContext = cidbContext;
        }
        public List<Country> GetCountries()
        {
            var country = _CidbContext.Countries.OrderBy(m => m.Name).ToList();
            return country;
        }

        public List<City> GetCities(List<int> id)
        {
            var cities = new List<City>();
            foreach (var countryId in id)
            {
                List<City> city = _CidbContext.Cities.Where(m => m.CountryId == countryId).OrderBy(o => o.Name).ToList();
                foreach (var c in city)
                {
                    cities.Add(c);
                }
            }
            return cities;
        }

        public List<Theme> GetThemes()
        {
            //1 means active, 2 means in-active
            var themes = _CidbContext.Themes.Where(t => t.DeletedAt == null && t.Status == 1).OrderBy(m => m.Title).ToList();
            return themes;
        }

        public List<Skill> GetSkills()
        {
            var skills = _CidbContext.Skills.Where(s => s.DeletedAt == null && s.Status == 1).OrderBy(m => m.SkillName).ToList();
            return skills;
        }

        public List<MissionRating> MissionRatings()
        {
            return _CidbContext.MissionRatings.ToList();
        }
        public LandingViewModel getCountryCityThemeSkills(string userid, int sortId, int pageIndex, int pageSize, string? keyword, List<long> countryids, List<long> cityids, List<long> themeids, List<long> skillids, int exploreId)

        {
            var countries = _CidbContext.Countries.ToList();
            var cities = _CidbContext.Cities.ToList();
            var themes = _CidbContext.Themes.ToList();
            int uid;
            if (userid == null)
            {
                uid = 0;
            }
            else
            {
                uid = int.Parse(userid);
            }
            var skill = _CidbContext.MissionSkills.Where(m => skillids.Contains(m.SkillId)).Select(m => m.MissionId);

            var missions = _CidbContext.Missions.Include(m => m.MissionRatings).Include(m => m.MissionApplications).Include(m => m.GoalMissions).Include(m => m.City).Include(m => m.Timesheets).Include(m => m.Theme).Include(m => m.FavouritMissions).Where(model => (keyword == null || model.Title.Contains(keyword) || model.Theme.Title.Contains(keyword) || model.City.Name.Contains(keyword)) && ((countryids.Contains(model.CountryId)) || countryids.Count() == 0) && ((cityids.Contains(model.CityId)) || cityids.Count() == 0) && (skill.Contains(model.MissionId) || skillids.Count() == 0) && ((themeids.Contains(model.ThemeId)) || themeids.Count() == 0) && model.DeletedAt == null && model.Status == true).AsQueryable();
            var missionSkill = _CidbContext.MissionSkills.Include(m => m.Skill).Include(m => m.Mission).ToList();

            var skill_drop = _CidbContext.Skills.ToList();
            var favmissions = _CidbContext.FavouritMissions.Where(m => m.UserId.ToString() == userid).ToList();
            var missionmedia = _CidbContext.MissionMedia.Include(m => m.Mission).ToList();
            var model = new LandingViewModel
            {

                GetCountryList = countries,
                GetCityList = cities,
                GetThemesList = themes,
                //Missions = missions.ToList(),
                MissionSkills = missionSkill,
                GetSkillList = skill_drop,
                totalrecord = missions.Count(),
                MissionRatings = _CidbContext.MissionRatings.ToList(),
                id = uid,
                missionMedia = missionmedia,
                favouritMissions = favmissions,
            };
            switch (exploreId)
            {
                case 1:
                    /*      var subquery = missions.GroupBy(mt => mt.ThemeId).Select(g => new { ThemeId = g.Key, Count = g.Count() });
                          missions = missions.Join(subquery, m => m.ThemeId, g => g.ThemeId, (m, g) => new { Mission = m, Count = g.Count })
                              .OrderByDescending(x => x.Count)
                              .Select(x => x.Mission);*/
                    missions = missions.Where(m => m.ThemeId != null && m.DeletedAt == null && m.Status == true).AsEnumerable().GroupBy(m => m.ThemeId).OrderByDescending(g => g.Count()).SelectMany(g => g).AsQueryable();
                    /*     missions=missions.AsQueryable().GroupBy(m=>m.ThemeId).OrderByDescending(m=>m.Count()).FirstOrDefault
                    */
                    break;

                case 2:
                    /*  missions= missions.Where(m=>m.CountryId != null && m.DeletedAt == null && m.Status == true).AsEnumerable().GroupBy(m=>m.CountryId).OrderByDescending(g => g.Count()).SelectMany(g=>g).AsQueryable();*/
                   /* missions = missions.GroupBy(m => m.MissionId).Select(g => new
              {
                  MissionId = g.Key,
                  AvgRating = Math.Ceiling(g.Average(m => m.MissionRatings.Where(m.MissionId == g.Key))

              })
              .OrderByDescending(g => g.SumRating)
              .AsQueryable();*/
                   missions = missions.OrderByDescending(m => m.MissionRatings.Where(r => r.MissionId == m.MissionId).Average(r => r.Rating)).AsQueryable();
                    break;

                case 3:
                    missions = missions.AsEnumerable().OrderByDescending(f => f.FavouritMissions.Count()).AsQueryable();
                    break;
                case 4:
                    Random random = new();

                    missions = missions.AsEnumerable().OrderBy(r=>random.Next()).AsQueryable();
                    break;         

            }

            /*    .Select(g => new { Title = g.Key, ThemeCount = g.Count() })
                .OrderByDescending(mt => mt.ThemeCount);*/

            var sorting = missions.ToList();
            if (sortId == 1)
            {
                sorting = sorting.OrderByDescending(p => p.StartDate).ToList();
            }
            else if (sortId == 2)
            {
                sorting = sorting.OrderBy(p => p.StartDate).ToList();
            }
            else if (sortId == 3)
            {
                sorting = sorting.OrderBy(p => p.Availability).ToList();
            }
            else if (sortId == 4)
            {
                sorting = sorting.OrderByDescending(p => p.Availability).ToList();
            }
            else if (sortId == 5)
            {
                sorting = sorting.OrderByDescending(p => favmissions.Any(m => m.MissionId == p.MissionId)).ToList();
            }
            else if (sortId == 6)
            {
                sorting = sorting.OrderBy(p => p.DeadlineDate).ToList();
            }
            model.GetMissionDetails = sorting.ToPagedList(pageIndex, 9);

            return model;
        }
  
        /* public List<Mission> GetMissionItems(string searchItems, string[] Items)
         {

             var missions = _CidbContext.Missions.Include(x => x.City).Include(x => x.Country).Include(x => x.Theme);
             var lstMissions = new List<Mission>();

             foreach (var mission in missions)
             {


                 if (!String.IsNullOrEmpty(searchItems))
                 {
                     if (mission.Title.ToLower().Contains(searchItems.ToLower()))
                     {
                         bool alreadyInList = missions.Contains(mission);
                         if (alreadyInList == false)
                         {
                             lstMissions.Add(mission);
                         }
                     }
                 }

                 if (Items != null)
                 {


                     foreach (var filter in Items)
                     {
                         if (filter == mission.City.Name || filter == mission.Theme.Title || filter == mission.Country.Name || filter==mission.Theme.Title)
                         {
                             bool alreadyInList = lstMissions.Contains(mission);
                             if (alreadyInList == false)
                             {
                                 lstMissions.Add(mission);
                             }
                         }
                     }

                 }

             }
             return lstMissions;
         }*/
        /*   public LandingViewModel getTopThemes()
           {
                           var query = context.Missions
                   .Join(context.Themes, m => m.ThemeId, t => t.ThemeId, (m, t) => new { Mission = m, Theme = t })
                   .GroupBy(mt => mt.Theme.Title)
                   .Select(g => new { Title = g.Key, ThemeCount = g.Count() })
                   .OrderByDescending(mt => mt.ThemeCount);

           }*/

    }

}

