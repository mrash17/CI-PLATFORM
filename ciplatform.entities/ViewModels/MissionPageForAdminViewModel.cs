using ciplatform.entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class MissionPageForAdminViewModel
    {
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public string Title { get; set; } = null!;



        public bool MissionType { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Select Start date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Select End date")]
        public DateTime? EndDate { get; set; }


        [Required(ErrorMessage = "Enter Short Description")]
        public string? ShortDescription { get; set; }
        public List<SelectListItem> countries { get; set; }
        public List<SelectListItem> cities { get; set; }
        public List<SelectListItem> Themes { get; set; }
  
        [Required(ErrorMessage = "Enter RegistrationDeadline Date")]
        public DateTime? RegistrationDeadline { get; set; }

/*        [Required(ErrorMessage = "Select Images")]
*/        public List<IFormFile>? Images { get; set; }
/*        [Required(ErrorMessage = "Select Document")]
*/        public List<IFormFile>? Documents { get; set; }
        [Required(ErrorMessage = "Select a country")]
        public long CountryId { get; set; }
        [Required(ErrorMessage = "Enter Status")]
        public bool? Status { get; set; }

        [Required(ErrorMessage = "Select a city")]
        public long CityId { get; set; }

        [Required(ErrorMessage = "Enter Organization Name")]
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "Enter Organization Detail")]
        public string OrganizationDetail { get; set; }
        public List<UserSkill> userSkills { get; set; }

        public List<Skill> Skills { get; set; }

        [Required(ErrorMessage = "Enter Goal Objective Text")]
        public string GoalObjectiveText { get; set; }

        [Required(ErrorMessage = "Enter Goal Value")]
        public int GoalValue { get; set; }


        public List<long> skillids { get; set; }



        [Required(ErrorMessage = "Enter Total Seats")]
        public long TotalSeats { get; set; }

        [Required(ErrorMessage = "Select Availability")]
        public string Availability { get; set; }

          

    }
}
