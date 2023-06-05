using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_PLATFORM.Controllers
{
    public class TimeSheetController : Controller
    {

        private readonly ILogger<TimeSheetController> _logger;

        private readonly ITimeSheetInterface _iTimeSheetInterface;
        public TimeSheetController(ITimeSheetInterface iTimeSheetInterface, ILogger<TimeSheetController> logger)
        {
            _iTimeSheetInterface = iTimeSheetInterface;
            _logger = logger;
        }
        public IActionResult volunteertimesheet()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            if (userid == null)
            {
                return RedirectToAction("Index", "User");
            }
            var timemissions = _iTimeSheetInterface.timeSheetViewModel(int.Parse(userid));

            return View(timemissions);
        }
        public JsonResult GetAllTimeBasedMissions()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            var getallmissions = _iTimeSheetInterface.getAllTimemissions(int.Parse(userid));
            return Json(getallmissions);
        }
        public JsonResult GetAllGoalBasedMissions()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            var getallmissions = _iTimeSheetInterface.getAllGoalmissions(int.Parse(userid));
            return Json(getallmissions);
        }

        public IActionResult SaveTimeMission(TimeSheetViewModel timeSheetViewModel)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");

             _iTimeSheetInterface.getAllTimemissions(timeSheetViewModel, userid);
            return RedirectToAction("volunteertimesheet");

        }
        public IActionResult DeleteTimeSheetData(int TimeSheetid )
            {
            var userid = HttpContext.Session.GetString("sessionuserid");

            _iTimeSheetInterface.deleteTimeSheetData(TimeSheetid, userid);
            return RedirectToAction("volunteertimesheet");

        }
        public JsonResult EditTimeSheet(int TimeSheetid)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
           var Timesheetdata= _iTimeSheetInterface.EditTimeSheetData(TimeSheetid, userid);
            return Json(Timesheetdata);
        }

        public IActionResult SaveEditedTimeMission(TimeSheetViewModel timeSheetViewModel)
        {
           
            var userid = HttpContext.Session.GetString("sessionuserid");
            var editedsheet = _iTimeSheetInterface.UpdateTimeSheet(timeSheetViewModel,userid);
            return RedirectToAction("volunteertimesheet");

        }

    }
}
