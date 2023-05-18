using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace ciplatform.repository.Repository
{
    public class StoryListingRepository : IStoryListingInterface
    {
        private readonly CidbContext _CidbContext;

        public StoryListingRepository(CidbContext cidbContext)
        {
            _CidbContext = cidbContext;
        }

        public StoryListingViewModel GetStories(string userid, string? SearchInputdata, int pageIndex, int pageSize)
        {//.Where( (keyword == null || missionIds.Contains(s.MissionId) || s.Title.Contains(keyword)))
            var misionApplied =_CidbContext.MissionApplications.Include(m=>m.Mission).Where(m=>m.UserId.ToString() == userid).ToList();
            //var mission = _CidbContext.Missions.Include(m=>m.us) .Include(m=>m.MissionApplications).Where(m=>m.u) .ToList();
            var gettingstories = _CidbContext.Stories.Include(m => m.StoryMedia).Include(m => m.User).Include(m => m.Mission.Theme).Include(m => m.Mission).Where(model => (SearchInputdata == null || model.Title.Contains(SearchInputdata)) && model.DeletedAt==null && model.Status==1).AsQueryable();
            // status = 1 means status is accepted
            var getstories = new StoryListingViewModel
            {
                stories = gettingstories.ToPagedList(pageIndex, pageSize),
                missions = misionApplied,
                totalstories= gettingstories.Count(),


            };
            return getstories;
        }

        public int SetStories(StoryListingViewModel StoryData,string userid, IFormFileCollection? FileInput)
        {
           
            if (StoryData == null)
            {
                return 0;
            }
            else
            {
                var newstory = new Story()
                {
                    UserId = int.Parse(userid),
                    MissionId = StoryData.StoryMissionId,
                    Title = StoryData.StoryTitle,
                    Description = StoryData.StoryDescription,
                    PublishedAt = StoryData.StoryDate,
                    Status = 2,
                };
                // status = 2 means status is pending

                foreach (IFormFile file in FileInput)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\storyimages", ImageName);
                    
                   

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        var storyMediumImg = new StoryMedium();
                        storyMediumImg.Type = file.ContentType.ToString().Replace("image/", "");
                        storyMediumImg.Path = "/storyimages/"+ImageName;
                        storyMediumImg.CreatedAt = DateTime.Now;
                        //storyMedium.StoryMediaId = storyListingView.StoryId;
                        newstory.StoryMedia.Add(storyMediumImg);
                        file.CopyTo(stream);
                    }
                }
                // var description = StoryData.StoryDescription.Substring(3, StoryData.StoryDescription.Length -4);

            
                _CidbContext.Add(newstory);
                _CidbContext.SaveChanges();

                var storyMedium = new StoryMedium()
                {
                    StoryId = newstory.StoryId,
                    Type = "videourl",
                    Path = StoryData.VideoPath,

                };
                _CidbContext.Add(storyMedium);
                _CidbContext.SaveChanges();
            }
                return 1;
        }
        public StoryDetailViewModel GetStoryDetails(int sid)
        {
            //   var story = _CidbContext.Stories.FirstOrDefault(u => u. == userid && u.Status == "DRAFT");
            var stories = _CidbContext.Stories.Where(m => m.StoryId == sid && m.Status==1);
            // status = 1 means status is accepted

            if (stories.Count() == 0)
            {
                return null;
            }

            var getStoryDetails = _CidbContext.Stories.Include(m => m.User).Include(m => m.StoryMedia).Where(m => m.StoryId == sid && m.Status==1).Select(m => new
            {
                Story = m,
                UserFirstName = m.User.FirstName,
                UserSecondname=m.User.SecondName,
                UserImage=m.User.Avatar,
                Storydescription=m.Description,
                whyIvolunteer=m.User.WhyIVolunteer,
                storytitle=m.Title,
                missionid=m.MissionId,
                viewcounts = m.Viewcount,


            }).ToList();
            var mediaurl = _CidbContext.StoryMedia.Where(m => m.StoryId == sid).ToList();
            
            var userdetails=_CidbContext.Users.ToList();

            var storydetails = new StoryDetailViewModel
            {
                FirstName=getStoryDetails.Select(m => m.UserFirstName).First(),
                SecondName=getStoryDetails.Select(m=>m.UserSecondname).First(),
                Avatar = getStoryDetails.Select(m => m.UserImage).First(),
                Description = getStoryDetails.Select(m => m.Storydescription).First(),
                WhyIVolunteer = getStoryDetails.Select(m => m.whyIvolunteer).First(),
                StoryTitle = getStoryDetails.Select(m => m.storytitle).First(),
                MissionId= getStoryDetails.Select(m => m.missionid).First(),
                Media= mediaurl,
                StoryId = sid,
                Viewcount = getStoryDetails.Select(m=>m.viewcounts).First(),

            };
            return storydetails;
        }
        public int GetViewCounts(int StoryId)
        {
            Story story = new Story();
            var GetStoryCount = _CidbContext.Stories.Where(m => m.StoryId == StoryId).FirstOrDefault();
            var CountView = GetStoryCount.Viewcount;
            CountView += 1;
            GetStoryCount.Viewcount = CountView;
            _CidbContext.Update(GetStoryCount);
            _CidbContext.SaveChanges();

            return 0;
        }


        public string GetUsersId(int id, string url, int from_id, int story_id)
        {
            int mid = id;

            var users = _CidbContext.Users.SingleOrDefault(m => m.UserId == mid);
            var from_email = new MailAddress("makhijashish2000@gmail.com", "Ashish");
            // jnptyjhdvcbbrlpp
            var to_email = new MailAddress(users.Email);
            var subject = "Recommeneded Story Page by " + users.FirstName;
            var body = $"Hi,<br /><br />Please click on the following to apply on the recommeneded Story:<br /><br /><a href='{url}'>{url}</a>";
            var message = new MailMessage(from_email, to_email)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("makhijashish2000@gmail.com", "jnptyjhdvcbbrlpp"),
                EnableSsl = true
            };
            smtpClient.Send(message);
            var storyinvite = new StotyInvite
            {
                StoryId = story_id,
                FromUserId = from_id,
                ToUserId = id
            };
            _CidbContext.Add(storyinvite);
            _CidbContext.SaveChanges();
            return "Successful";
        }




    }
}
