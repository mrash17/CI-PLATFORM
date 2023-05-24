using ciplatform.entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ciplatform.entities.ViewModels
{
    public class StoryListingViewModel
    {
        public IPagedList<Story> stories { get; set; }
        public int totalstories { get; set; }
        public List<MissionApplication> missions { get; set; }

        public long StoryUserId { get; set; }
        public long StoryId { get; set; }

        public long StoryMissionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Title")]
        public string? StoryTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter StoryDescription")]
        public string? StoryDescription { get; set; }

        public int Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Date")]
        public DateTime? StoryDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Url")]
        public string VideoPath { get; set; } = null!;
        public string ImagesPath { get; set; } = null!;



    }
}
