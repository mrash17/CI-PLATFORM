using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CI_PLATFORM.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IAdminInterface _iAdminInterface;
        public AdminController(IAdminInterface iAdminInterface, ILogger<AdminController> logger)
        {
            _iAdminInterface = iAdminInterface;
            _logger = logger;
        }
        public IActionResult AdminPage()
        {
            return View();
        }



        //User
        [Authorize(Roles = "Admin")]
        public IActionResult UserPage(string searchkeyword = "", int pageIndex = 1)
        {
            /*var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }*/


            var userList = _iAdminInterface.getUsers(searchkeyword, pageIndex);

            return PartialView("_adminUserPage", userList);
        }
        public IActionResult AccessDenied()
        {
            return RedirectToAction("PageNotFound", "Home");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveUserByAdmin(string userid)
        {

            var dUser = _iAdminInterface.RemoveUser(int.Parse(userid));
            return PartialView("_adminUserPage");
        }
        public IActionResult addUser()
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            return PartialView("_adminAddUser");

        }
        [HttpPost]
        public IActionResult addUser(UserPageForAdminViewModel AdduserPageForAdminView, IFormFileCollection formFilesImg)
        {
            if (AdduserPageForAdminView.UserId == 0)
            {
                var emailchk = _iAdminInterface.AddUser(AdduserPageForAdminView, formFilesImg);
                if (emailchk == 1)
                    TempData["UserAdded"] = "User Added successfully";
                else if (emailchk == 0)
                {
                    TempData["UserEdited"] = "Email id Already Exsites";
                    return PartialView("_adminAddUser", AdduserPageForAdminView);
                }

            }
            else
            {
                _iAdminInterface.EditUser(AdduserPageForAdminView, formFilesImg);
                TempData["UserEdited"] = "User Edited successfully";

            }
            return RedirectToAction("UserPage");
        }

        public IActionResult EditUser(int id)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }
            var userData = _iAdminInterface.GetUserDetail(id);

            return PartialView("_adminAddUser", userData);

        }

        //Missions View
        public IActionResult MissionPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var missionsList = _iAdminInterface.getMissions(searchkeyword, pageIndex);
            return PartialView("_adminMissionPage", missionsList);
        }
        public IActionResult RemoveMissionByAdmin(int Missionid)
        {
            var dMission = _iAdminInterface.RemoveMission(Missionid);
            return PartialView("_adminMissionPage");
        }

        // Add Edit Missions
        [Authorize(Roles = "Admin")]
        public IActionResult missionadd()
        {
            var model = _iAdminInterface.getmissionmodeldata();
            return PartialView("_addEditMission", model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddMission(MissionPageForAdminViewModel model, List<int> selectedSkills)
        {
            if (model.MissionId == 0)
            {
                _iAdminInterface.Addmission(model, selectedSkills);
                TempData["mission"] = "Mission Added Successfully";
            }
            else
            {
                _iAdminInterface.Editmission(model, selectedSkills);
                TempData["mission"] = "Mission Edited Successfully";
            }

            return RedirectToAction("MissionPage");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult editmission(string id)
        {
            var model = _iAdminInterface.editmissondata(id);
            return PartialView("_addEditMission", model);
        }
        public JsonResult GetThemes()
        {
            var themes = _iAdminInterface.getthemes();
            return Json(themes);
        }


        //MissionApplications
        public IActionResult MissionApplicationPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var MissionApplicationList = _iAdminInterface.getApplicationsOfMission(searchkeyword, pageIndex);
            return PartialView("_adminMissionapplication", MissionApplicationList);
        }
        public IActionResult ApprovalMissionApplicationByAdmin(int MissionApplicationId, string approvalstatus)
        {
            _iAdminInterface.ChangeApplicationStatus(MissionApplicationId, approvalstatus);
            return PartialView("_adminMissionapplication");
        }

        //Story for Admin
        public IActionResult StoryPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var StoriesList = _iAdminInterface.getStories(searchkeyword, pageIndex);
            return PartialView("_adminStoryPage", StoriesList);
        }
        public IActionResult StoryStats(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var StoriesList = _iAdminInterface.getStories(searchkeyword, pageIndex);
            return PartialView("_statistics", StoriesList);
        }

        public JsonResult storyStatsList()
        {
            var StoriesList = _iAdminInterface.getStories("", 1);
            return Json(StoriesList);   
        }
        public IActionResult ApprovalStoryByAdmin(int StoryId, int approvalstatus)
        {
            _iAdminInterface.ChangeStoryStatus(StoryId, approvalstatus);
            return PartialView("_adminStoryPage");
        }
        public IActionResult RemoveStoryByAdmin(int StoryId)
        {
            var dStory = _iAdminInterface.RemoveStory(StoryId);
            return PartialView("_adminStoryPage");
        }



        //Theme for Admin
        public IActionResult ThemePage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var ThemeList = _iAdminInterface.getThemes(searchkeyword, pageIndex);
            return PartialView("_adminThemePage", ThemeList);
        }
        public IActionResult RemoveThemeByAdmin(int ThemeId)
        {
            var dTheme = _iAdminInterface.RemoveTheme(ThemeId);
            // 0 means missiona and present with asked theme
            if (dTheme == 0)
            {
                return PartialView("_adminThemePage", dTheme);

            }
            else
            {
                return PartialView("_adminThemePage");

            }
        }

        public IActionResult EditTheme(int themeid)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }
            if (themeid == 0)
            {
                return PartialView("_adminAddTheme");

            }
            var ThemeData = _iAdminInterface.GetThemeData(themeid);

            return PartialView("_adminAddTheme", ThemeData);

        }
        [HttpPost]
        public IActionResult EditTheme(ThemePageForAdminViewModel themePageForAdminView)
        {
            if (themePageForAdminView.ThemeId != 0)
            {
                var theme_chk = _iAdminInterface.EditTheme(themePageForAdminView);
                if (theme_chk == 2)
                {
                    TempData["ThemeNotEdted"] = "This Theme Title is already present";
                    return PartialView("_adminAddTheme", themePageForAdminView);
                }
                if (theme_chk == 1)
                {
                    TempData["ThemeEdted"] = "Theme Edited successfully";

                }
                else
                {
                    TempData["ThemeNotEdted"] = "Status cannot be Changed as it is being used";

                }
                return RedirectToAction("ThemePage");


            }
            else
            {
                var Titlechk = _iAdminInterface.AddTheme(themePageForAdminView);
                if (Titlechk != 0)
                {
                    TempData["ThemeNotEdted"] = "This Theme is Already present";
                    return RedirectToAction("ThemePage");
                }
                else
                {
                    TempData["ThemeNotEdted"] = "This Theme is Already present";
                    return PartialView("_adminAddTheme", themePageForAdminView);
                }
            }
        }

        // Skill page For Admin
        public IActionResult SkillPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var SkillList = _iAdminInterface.getSkills(searchkeyword, pageIndex);
            return PartialView("_adminSkillPage", SkillList);
        }
        public IActionResult RemoveSkillByAdmin(int skillId)
        {
            var dSkill = _iAdminInterface.RemoveSkill(skillId);
            // 0 means mission is present with asked theme
            if (dSkill == 0)
            {
                return PartialView("_adminSkillPage", dSkill);

            }
            else
            {
                return PartialView("_adminSkillPage");

            }
        }
        public IActionResult EditSkill(int skillid)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }
            if (skillid == 0)
            {
                return PartialView("_adminAddSkill");

            }
            var ThemeData = _iAdminInterface.GetSkillData(skillid);

            return PartialView("_adminAddSkill", ThemeData);

        }

        [HttpPost]
        public IActionResult EditSkill(SkillPageForAdminViewModel skillPageForAdmin)
        {
            if (skillPageForAdmin.Skillid != 0)
            {
                var skill_chk = _iAdminInterface.EditSkill(skillPageForAdmin);
                if (skill_chk == 2)
                {
                    TempData["SkillNotEdited"] = "This Skill Title is already present";
                    return PartialView("_adminAddSkill", skillPageForAdmin);
                }
                if (skill_chk == 1)
                {
                    TempData["SkillEdited"] = "Skill Edited successfully";

                }
                if (skill_chk == 3)
                {
                    TempData["NoChangesDone"] = "No Changes done";

                }
                else
                {
                    TempData["SkillNotEdited"] = "Status cannot be Changed as it is being used";

                }
                return RedirectToAction("SkillPage");


            }
            else
            {
                var Titlechk = _iAdminInterface.AddSkill(skillPageForAdmin);
                if (Titlechk != 0)
                {
                    TempData["SkillEdited"] = "Skill Added successfully";
                    return RedirectToAction("SkillPage");
                }
                else
                {
                    TempData["SkillNotEdited"] = "This Skill is Already present";
                    return PartialView("_adminAddSkill", skillPageForAdmin);
                }
            }

        }
        //bannerPage
        public IActionResult BannerPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var BannerList = _iAdminInterface.getBanners(searchkeyword, pageIndex);
            return PartialView("_adminBanner", BannerList);
        }

        public IActionResult RemoveBannerByAdmin(int bannerId)
        {
            var dBanner = _iAdminInterface.RemoveBanner(bannerId);
            // 0 means missiona and present with asked theme
            if (dBanner == 0)
            {
                return PartialView("_adminBanner", dBanner);

            }
            else
            {
                return PartialView("_adminBanner");

            }
        }

        public IActionResult AddEditBanner(int bannerId)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }
            if (bannerId == 0)
            {
                return PartialView("_adminAddEditBanner");

            }
            var ThemeData = _iAdminInterface.GetBannerData(bannerId);

            return PartialView("_adminAddEditBanner", ThemeData);

        }

        [HttpPost]
        public IActionResult AddEditBanner(BannerPageForAdminViewModel bannerPageForAdminView, IFormFileCollection IFormImg)
        {
            if (bannerPageForAdminView.BannerId != 0)
            {
                _iAdminInterface.EditBanner(bannerPageForAdminView, IFormImg);

                TempData["SkillEdited"] = "Banner Edited successfully";

                return RedirectToAction("BannerPage");


            }
            else
            {
                _iAdminInterface.AddBanner(bannerPageForAdminView, IFormImg);

                TempData["SkillEdited"] = "Banner Added successfully";
                return RedirectToAction("BannerPage");

            }

        }
        //CMS Page For Admin
        public IActionResult CMSPage(string searchkeyword = "", int pageIndex = 1)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }

            var CMSList = _iAdminInterface.getCMSList(searchkeyword, pageIndex);
            return PartialView("_adminCMSPage", CMSList);
        }
        public IActionResult AddEditCMS(int cmsId)
        {
            var adminid = HttpContext.Session.GetString("sessionAdminid");

            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid != null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            else if (adminid == null)
            {
                return RedirectToAction("Index", "User");

            }
            if (cmsId == 0)
            {
                return PartialView("_adminAddEditCMS");

            }
            var CMSdata = _iAdminInterface.GetCMSData(cmsId);

            return PartialView("_adminAddEditCMS", CMSdata);

        }
        [HttpPost]
        public IActionResult AddEditCMS(CMSPageForAdminViewModel cmsPageForAdminViewModel)
        {
            cmsPagePost(cmsPageForAdminViewModel);

            if (cmsPageForAdminViewModel.CmsPageId != 0)
            {

                TempData["SkillEdited"] = "CMS Edited successfully";
            }
            else
            {
                TempData["SkillEdited"] = "CMS Added successfully";
            }
            return RedirectToAction("CMSPage");



        }

        private static void cmsPagePost(CMSPageForAdminViewModel cmsPageForAdminViewModel)
        {
            string apiUrl = "https://localhost:44381/api/CustomAPI/AddEditCms";

            HttpClient client = new HttpClient();

            //Converting data into json for passing it
            string userJson = JsonConvert.SerializeObject(cmsPageForAdminViewModel);
            //To convert userJon to httpcontent
            HttpContent content = new StringContent(userJson, Encoding.UTF8, "application/json");
            // Post Request to API
            HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("CMS added successfully.");
            }
            else
            {
                Console.WriteLine("Error adding user: " + response.ReasonPhrase);
            }
        }

        public IActionResult RemoveCMSByAdmin(int cmsId)
        {
            var dCMS = _iAdminInterface.RemoveCMS(cmsId);
            return PartialView("_adminCMSPage");
        }

       public IActionResult RemoveSelectedCms(List<int> checkedCmsValues)
        {
            var dCheckedCms = _iAdminInterface.removeCheckedcms(checkedCmsValues);
            return PartialView("_adminCMSPage");

        }
    }
}
