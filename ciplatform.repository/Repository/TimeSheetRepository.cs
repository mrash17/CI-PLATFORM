using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Repository
{
    public class TimeSheetRepository : ITimeSheetInterface
    {
        private readonly CidbContext _CidbContext;

        public TimeSheetRepository(CidbContext cidbContext)
        {
            _CidbContext = cidbContext;
        }
        public List<Mission> getAllTimemissions(int userid)
        {
            var missionsapplications = _CidbContext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.UserId == userid && m.Mission.MissionType== true && m.ApprovalStatus == "approved").Select(m=>m.MissionId);
            var missions = _CidbContext.Missions.Where(m => missionsapplications.Contains(m.MissionId) && m.Status == true && m.DeletedAt == null).OrderBy(m => m.Title).ToList();
            
            return missions;
        }
        public List<Mission> getAllGoalmissions(int userid)
        {
            var missionsapplications = _CidbContext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.UserId == userid && m.Mission.MissionType == false && m.ApprovalStatus == "approved").Select(m => m.MissionId);
            var missions = _CidbContext.Missions.Where(m => missionsapplications.Contains(m.MissionId) && m.Status == true && m.DeletedAt == null).OrderBy(m => m.Title).ToList();
            return missions;
        }
        public TimeSheetViewModel timeSheetViewModel(int userid)
        {
            var timebasedmissions = _CidbContext.Timesheets.Include(m=>m.Mission).Where(m => m.Mission.MissionType == true && m.UserId==userid).ToList();
            var goalbasedmissions = _CidbContext.Timesheets.Include(m => m.Mission).Where(m => m.Mission.MissionType == false && m.UserId == userid).ToList();
            var timesheetviewmodel = new TimeSheetViewModel
            {
                missiontimesheet = timebasedmissions,
                missiongoalsheet = goalbasedmissions,
            };
            return timesheetviewmodel;
        }

        public void getAllTimemissions(TimeSheetViewModel model, string userid)
        {
            Timesheet timesheet = new Timesheet()
            {
                UserId=int.Parse(userid),
                MissionId=model.missionid,
                Time =  DateTime.MinValue.AddHours(model.hour).AddMinutes(model.minute),
                DateVolunteer = model.date,
                Notes=model.message,
                Action=model.Action,
                Status=true,
            };
            _CidbContext.Add(timesheet);
            _CidbContext.SaveChanges();
        }
        public void deleteTimeSheetData(int timesheetid, string userid)
        {
            var timesheetdata= _CidbContext.Timesheets.FirstOrDefault(d=>d.UserId==int.Parse(userid) && d.TimesheetId== timesheetid);
            _CidbContext.Remove(timesheetdata);
            _CidbContext.SaveChanges();

        }
        public TimeSheetViewModel EditTimeSheetData(int timesheetid, string userid)
        {
            var timesheetdata= _CidbContext.Timesheets.Include(m=>m.Mission).FirstOrDefault(d=>d.UserId==int.Parse(userid) && d.TimesheetId== timesheetid);
            DateTime date = (DateTime)timesheetdata.Time;

/*            string hour = DateTime.Now.ToString("HH");
*/            var timesheet = new TimeSheetViewModel()
            {

                hour = date.Hour,
                minute = date.Minute,
                missionid = (int)timesheetdata.MissionId,
                date = timesheetdata.DateVolunteer,
                MissiontTitle=timesheetdata.Mission.Title,
                message = timesheetdata.Notes,
                Action = timesheetdata.Action,
            };
           
            return timesheet;
        }

        public int UpdateTimeSheet(TimeSheetViewModel timeSheetViewModel, string userid)
        {
            if (timeSheetViewModel == null)
            {
                return 0;
            }
            else
            {
                var timesheetid = timeSheetViewModel.timesheetid;
                var TimeSheet = _CidbContext.Timesheets.Where(m => m.TimesheetId == timesheetid && m.UserId == int.Parse(userid)).FirstOrDefault();
                if (TimeSheet.Action == null)
                {
                    TimeSheet.Time = DateTime.MinValue.AddHours(timeSheetViewModel.hour).AddMinutes(timeSheetViewModel.minute);
                }

                else
                {
                    TimeSheet.Action = timeSheetViewModel.Action;
                }
                TimeSheet.Notes = timeSheetViewModel.message;
                TimeSheet.DateVolunteer = timeSheetViewModel.date;
                _CidbContext.SaveChanges();

                return 1;
            }

        }
    }
}
