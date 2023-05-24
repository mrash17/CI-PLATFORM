using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class BannerPageForAdminViewModel
    {
        public long BannerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter title")]
        public string? Title { get; set; }

        public string Image { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Some text")]
        public string? Text { get; set; }

        public bool? Status { get; set; }
        [Required(ErrorMessage = "Please Enter Sort Order")]
        public int? SortOrder { get; set; }
    }
}
