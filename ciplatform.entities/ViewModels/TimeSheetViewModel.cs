using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class TimeSheetViewModel
    {
        public int missionid { get; set; }
        public int timesheetid { get; set; }
        public DateTime date { get; set; }

        public int hour { get; set; }
        public int minute { get; set; }

        public string message { get; set; }

        public string MissiontTitle { get; set; }
        public int? Action { get; set; }

        public List<Timesheet> missiontimesheet { get; set; } = new List<Timesheet>();
        public List<Timesheet> missiongoalsheet { get; set; } = new List<Timesheet>();

    }
}
