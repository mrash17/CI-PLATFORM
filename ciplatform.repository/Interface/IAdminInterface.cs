using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ciplatform.repository.Interface
{
    public interface IAdminInterface
    {
        public IPagedList<UserPageForAdminViewModel> getUsers(string searchkeyword, int pageIndex);
       
        public int AddUser(UserPageForAdminViewModel AdduserPageForAdminView, IFormFileCollection formFilesImg);
        public void EditUser(UserPageForAdminViewModel AdduserPageForAdminView, IFormFileCollection formFilesImg);
        public int RemoveUser(int userid);
        public UserPageForAdminViewModel GetUserDetail(int id);

        //Missions
        public IPagedList<MissionPageForAdminViewModel> getMissions(string searchkeyword, int pageIndex);
        public int RemoveMission(int Missionid);

        public List<Theme> getthemes();
        public MissionPageForAdminViewModel getmissionmodeldata();
        public void Addmission(MissionPageForAdminViewModel model, List<int> selectedSkills);
        public MissionPageForAdminViewModel editmissondata(string missonid);
        public void Editmission(MissionPageForAdminViewModel model, List<int> selectedSkills);

        //Mission Applications
        public IPagedList<MissionApplicationForAdminViewModel> getApplicationsOfMission(string searchkeyword, int pageIndex);
        public void ChangeApplicationStatus(int MissionApplicationId,string approvalstatus);
        //Stories
        public IPagedList<StoryPageForAdminViewModel> getStories(string searchkeyword, int pageIndex,int min,int max);
        public void ChangeStoryStatus(int StoryId, int approvalstatus);
        public int RemoveStory(int StoryId);
            public Story updateStoryByDTb(string storyid, string columnName, string columndata);


        //Theme
        public IPagedList<ThemePageForAdminViewModel> getThemes(string searchkeyword, int pageIndex);
        public int RemoveTheme(int ThemeId);
        
        public ThemePageForAdminViewModel GetThemeData(int themeid);

        public int AddTheme(ThemePageForAdminViewModel themePageForAdminView);
        public int EditTheme(ThemePageForAdminViewModel themePageForAdminView);

        //Skills 
        public IPagedList<SkillPageForAdminViewModel> getSkills(string searchkeyword, int pageIndex);
        public int RemoveSkill(int skillId);
        public SkillPageForAdminViewModel GetSkillData(int skillId);

        public int AddSkill(SkillPageForAdminViewModel skillPageForAdminViewModel);
        public int EditSkill(SkillPageForAdminViewModel skillPageForAdminViewModel);

        //Banner
        public IPagedList<BannerPageForAdminViewModel> getBanners(string searchkeyword, int pageIndex);
        public BannerPageForAdminViewModel GetBannerData(int bannerId);
        public int RemoveBanner(int bannerId);
        public void AddBanner(BannerPageForAdminViewModel bannerPageForAdminView, IFormFileCollection IFormImg);
        public void EditBanner(BannerPageForAdminViewModel bannerPageForAdminView, IFormFileCollection IFormImg);

        //CMS Page
        public IPagedList<CMSPageForAdminViewModel> getCMSList(string searchkeyword, int pageIndex);
        public CMSPageForAdminViewModel GetCMSData(int cmsId);
        public void EditCMS(CMSPageForAdminViewModel cmsPageForAdminViewModel);
        public void AddCMS(CMSPageForAdminViewModel cmsPageForAdminViewModel);
        public int RemoveCMS(int cmsId);
        public int removeCheckedcms(List<int> checkedCmsValues);


        //Excel to sql
        public MappingColumns getColumnsMap(IFormFile file);
        public int exportToSql(DataTable dataTable, List<string> selectedIndicesList);
    }
}
