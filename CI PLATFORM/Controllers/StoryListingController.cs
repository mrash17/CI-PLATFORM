using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_PLATFORM.Controllers
{
    public class StoryListingController : Controller
    {


        private readonly ILogger<StoryListingController> _logger;

        private readonly IStoryListingInterface _iStoryListingInterface;

        public StoryListingController(IStoryListingInterface iStoryListingInterface, ILogger<StoryListingController> logger)
        {
            _iStoryListingInterface = iStoryListingInterface;
            _logger = logger;
        }
        public IActionResult storylisting(string? SearchInputdata = "", int pageIndex = 1, int pageSize = 9)
        {

            var userid = HttpContext.Session.GetString("sessionuserid");

           
                var storyoutput = _iStoryListingInterface.GetStories(userid, SearchInputdata,pageIndex, pageSize);

                return View(storyoutput);
            
        }
        [Authorize]
        [HttpGet]
        public IActionResult shareyourstory()
        {
            
                var userid = HttpContext.Session.GetString("sessionuserid");


                if (userid == null)
                {
                    return RedirectToAction("Index", "User");

                }
                else
                {
                    var storymissions = _iStoryListingInterface.GetStories(userid, "",1, 9);
                    return View(storymissions);
                }
            
        }
       
        [HttpPost]
        public IActionResult shareyourstory(StoryListingViewModel StoryData, IFormFileCollection? FileInput)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");

            var storymissions = _iStoryListingInterface.GetStories(userid, "", 1, 9);
            

                if (userid == null)
                {
                    return RedirectToAction("Index", "User");

                }
                else
                {
                    var storydata = _iStoryListingInterface.SetStories(StoryData, userid,FileInput);
                    return RedirectToAction("storylisting", "StoryListing");
                }
           

        }
        public IActionResult storydetail(int sid)
        {
            var storyresult = _iStoryListingInterface.GetStoryDetails(sid);
            if(storyresult == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(storyresult);

        }
        public IActionResult GetViewCount(int StoryId)
        {
            _iStoryListingInterface.GetViewCounts(StoryId);
            return Json(new { redirectUrl = Url.Action("storydetail", "StoryListing", new { sid = StoryId }) });
        }
        public string SendmailByUserid(int[] ids, int storyid, int from_id)
        {
            foreach (var mid in ids)
            {
                string url = Url.Action("storydetail", "StoryListing", new { sid = storyid }, Request.Scheme);
                var userids = _iStoryListingInterface.GetUsersId(mid, url, from_id, storyid);
            }
            return "Successfull";
        }
    }
}
