using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Interface
{
    public interface IStoryListingInterface
    {
/*        public List<Story> GetStoryListing(string Userid);
*/        public StoryListingViewModel GetStories (string userid, string? SearchInputdata,int pageIndex , int pageSize);
          public int SetStories(StoryListingViewModel StoryData,string userid, IFormFileCollection? FileInput);
        public StoryDetailViewModel GetStoryDetails(int sid);
        public string GetUsersId(int id, string url, int from_id, int story_id);
        public int GetViewCounts(int StoryId);



    }
}
