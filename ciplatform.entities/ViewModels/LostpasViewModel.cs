using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class LostpasViewModel
    {
        public List<Banner> bannerList { get; set; }

        [Required(ErrorMessage = "Enter required Email")]
        public string? Email { get; set; }
    }
}
