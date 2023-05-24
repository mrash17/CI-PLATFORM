using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class SkillPageForAdminViewModel
    {
        public long Skillid { get; set; }

        public byte Status { get; set; }
        [Required(ErrorMessage = "Skill Title is required")]
        public string SkillTitle { get; set; } = null!;

    }
}
