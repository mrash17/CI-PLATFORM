using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class UserPageForAdminViewModel
    {
        public long UserId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please select a country")]
        public long Countryid { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a City")]
        public long Cityid { get; set; }
        public string? Countryname { get; set; }
        public string? CityName { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Description")]
        public string ProfileText { get; set; } = null!;
        public string? EmployeeId { get; set; }
        public string? Department { get; set; }
     

        [Required(ErrorMessage = "Select a option")]
        public bool? Status { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Phone no. is required")]
        [RegularExpression("[0-9]{10}$", ErrorMessage = "Invalid Mobile.no")]
        public int PhoneNumber { get; set; }
        public string? Avatar { get; set; }


    }
}
