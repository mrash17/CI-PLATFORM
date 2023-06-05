using ciplatform.entities.Data;
using Microsoft.AspNetCore.Mvc;
using ciplatform.entities.Models;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using ciplatform.entities.ViewModels;

namespace CI_PLATFORM.Controllers
{
    
    public class LandingPageController : Controller
    {
        private readonly ILogger<LandingPageController> _logger;

        private readonly ILandingPageInterface _ilandingPageInterface;
        private readonly IVolunteerMissionInterface _iVolunteerMissionInterface;

        public LandingPageController(ILandingPageInterface ilandingPageInterface, IVolunteerMissionInterface iVolunteerMissionInterface, ILogger<LandingPageController> logger)
        {
            _ilandingPageInterface = ilandingPageInterface;
            _iVolunteerMissionInterface = iVolunteerMissionInterface;
            _logger = logger;
        }


         public IActionResult landingpage(List<long> skillids, List<long> themeids, List<long> cityids, List<long> countryids, int sortId = 0, int pageIndex = 1, int pageSize = 9, string? SearchInputdata = "",int exploreId = 0)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            var entity = _ilandingPageInterface.getCountryCityThemeSkills(userid ,sortId, pageIndex, pageSize, SearchInputdata, countryids, cityids, themeids, skillids,exploreId);
            entity.currentPage = pageIndex;
            return View(entity);

        }
        public JsonResult Country()
        {
            var country = _ilandingPageInterface.GetCountries();
            return Json(country);
        }

        public JsonResult City(List<int> id)
        {
            var city = _ilandingPageInterface.GetCities(id);
            return Json(city);
        }

        public JsonResult Theme()
        {
            var themes = _ilandingPageInterface.GetThemes();
            return Json(themes);
        }

        public JsonResult Skill()
        {
            var skills = _ilandingPageInterface.GetSkills();
            return Json(skills);
        }




        [HttpGet]
        public IActionResult VolunteeringMission(int mid)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
           /* if(userid==null)
            {
                return RedirectToAction("Index", "User");
            }*/
            var missiomdetails = _iVolunteerMissionInterface.GetVolunteerMission(mid, userid);
            if(missiomdetails == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            /*            var mission = _Cidbcontext.Missions.Where(model => model.MissionId == mid).FirstOrDefault();
            */
            return View(missiomdetails);
        }
        
        //Update Ratings

        public IActionResult updaterating(int missionid, int rating, int userid)
        {
            var sessionstring = HttpContext.Session.GetString("sessionuserid");
            if (sessionstring == null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                var success_rate = _iVolunteerMissionInterface.updateandrate(missionid, rating, userid);
                return RedirectToAction("VolunteeringMission", new { mid = missionid });
            }

        }

        public IActionResult favouritelanding(int userid, int missionid)
        {
            var Favourite = _iVolunteerMissionInterface.chkfavourite(userid, missionid);
            return RedirectToAction("landingpage","LandingPage");
        }
        public IActionResult favourite(int userid, int missionid)
        {
            var Favourite = _iVolunteerMissionInterface.chkfavourite(userid, missionid);
            return RedirectToAction("VolunteeringMission", new { mid = missionid });
        }


        public JsonResult getusers(int Userid)
        {
           
            var users = _iVolunteerMissionInterface.GetUsersbyid(Userid);
            return Json(users);
        }
        public string SendmailByUserid(int[] ids, int missionid,int from_id)
        {
            foreach(var mid in ids)
            {
                string url = Url.Action("VolunteeringMission", "LandingPage", new { mid = missionid }, Request.Scheme);
                var userids = _iVolunteerMissionInterface.GetUsersId(mid, url, from_id, missionid);
            }
            return "Successfull";
        }
        public IActionResult UserCommentPost(string commentstext,int MissionId,int userId)
        {
           
            var usercommentpost=_iVolunteerMissionInterface.UserCommentPost(commentstext,MissionId,userId);
            return RedirectToAction("VolunteeringMission", new { mid = MissionId });

        }
        public IActionResult ApplyingMission(string userid, int MissionId)
        {
            var missionapplied = _iVolunteerMissionInterface.applymission(userid, MissionId);
            return RedirectToAction("VolunteeringMission", new { mid = MissionId });
        }

    /*    public IActionResult GetTopThemes()
        {
            var topThemes = _ilandingPageInterface.getTopThemes();
            return View(topThemes);
        }*/
    }
}
