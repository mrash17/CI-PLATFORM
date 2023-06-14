using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class UserDetailsViewModel
    {
        public long UserId { get; set; }

        public long Countryid { get; set; }
        public long Cityid { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }
        public string? EmployeeId { get; set; }
        public string? Title { get; set; }

        public string? Department { get; set; }
        public string? Manager { get; set; }
        public string? ProfileText { get; set; }

        public string? WhyIVolunteer { get; set; }
        public string? Avatar { get; set; }

        public string? Availability { get; set; }



        public string? LinkedInUrl { get; set; }

        public List<UserSkill> userSkill { get; set; } 
        public List<Skill> Skills { get; set; } 

        public string CountryName { get; set; } = null!;
        public string CityName { get; set; } = String.Empty;
        public List<int> userselectedSkills { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
/*        [Compare("Password", ErrorMessage = "Password cannot be same")]
*/        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password")]
        public string NewPassword { get; set; } = null!;

        [NotMapped]
        [Required(ErrorMessage = "Confirm Passowrd is required")]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not match")]
        public string? ConfirmPassword { get; set; }


    }
}
