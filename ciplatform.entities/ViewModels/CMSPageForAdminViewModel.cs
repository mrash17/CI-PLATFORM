using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class CMSPageForAdminViewModel
    {
        public long CmsPageId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter title")]
        public string? Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Description")]
        public string? Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Slug")]
        public string Slug { get; set; } = null!;

        public bool? Status { get; set; }
/*        public List<CmsPage> cmspage { get; set; }
*/    }
}
