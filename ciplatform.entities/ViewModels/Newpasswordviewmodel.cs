using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModel
{
    public class Newpasswordviewmodel
    {
        public List<Banner> bannerList { get; set; }
        public String? Email { get; set; }
        public String? Token { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]   
        [NotMapped]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must be same")]
        public string? ConfirmPassword { get; set; }

    }
}
