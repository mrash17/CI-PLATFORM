using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ciplatform.repository.Repository;
using ciplatform.entities.ViewModels;

namespace ciplatform.repository.Interface
{
    public interface ILandingPageInterface
    {
        public LandingViewModel getCountryCityThemeSkills(string userid, int id, int pageIndex, int pageSize, string? keyword, List<long> countryids, List<long> cityids, List<long> themeids, List<long> skillids,int exploreId);

        public List<Country> GetCountries();


        public List<City> GetCities(List<int> id);

        public List<Theme> GetThemes();
        public List<Skill> GetSkills();

        // List<Mission> GetMissionDetails();

/*            public LandingViewModel getTopThemes();
*/

    }
}
