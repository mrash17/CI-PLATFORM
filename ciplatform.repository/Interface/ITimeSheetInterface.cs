using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Interface
{
    public interface ITimeSheetInterface
    {
        public List<Mission> getAllTimemissions(int userid); 
        public List<Mission> getAllGoalmissions(int userid);
        public TimeSheetViewModel timeSheetViewModel(int userid);
        public void getAllTimemissions(TimeSheetViewModel model, string userid);
        public void deleteTimeSheetData(int timesheetid, string userid);
        public int UpdateTimeSheet(TimeSheetViewModel timeSheetViewModel, string userid);
        public TimeSheetViewModel EditTimeSheetData(int timesheetid, string userid);
        

    }
}
