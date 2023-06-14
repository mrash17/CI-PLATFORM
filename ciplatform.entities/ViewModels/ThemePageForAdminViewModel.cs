using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class ThemePageForAdminViewModel
    {
        public long ThemeId { get; set; }

        public byte Status { get; set; }
        [Required(ErrorMessage = "Theme Title is required")]
        public string ThemeTitle { get; set; } = null!;

    }
}
