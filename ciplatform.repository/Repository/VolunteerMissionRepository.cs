using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Repository
{
    public class VolunteerMissionRepository : IVolunteerMissionInterface
    {
        private readonly CidbContext _CidbContext;

        public VolunteerMissionRepository(CidbContext cidbContext)
        {
            _CidbContext = cidbContext;
        }

        public VolunteeringMissionViewModel GetVolunteerMission(int mid, string userid)
        {


            //Fetching Matching mission
            Mission mission = _CidbContext.Missions.Include(m=>m.MissionMedia).Include(m => m.City).Include(m => m.Theme).Include(m => m.Country).SingleOrDefault(m => m.MissionId == mid);
            var missionsMedia = mission.MissionMedia.Where(media=>media.MissionId == mid).ToList(); 
            if(mission==null)
            {
                return null;
            }
            //fetching skill name
            List<string> skills = _CidbContext.MissionSkills.Where(m => m.MissionId == mid).Include(m => m.Mission).Include(m => m.Skill).Select(m => m.Skill.SkillName).ToList();
            var missionapplications = _CidbContext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.MissionId == mid && m.User.Status == true && m.User.DeletedAt == null).ToList();
            //comments display
            var comment_display = _CidbContext.Comments.Include(m => m.Mission).Include(m => m.User).Where(m => m.MissionId == mid).OrderByDescending(m => m.CreatedAt).ToList();
            //Fetching Related mission
            var relatedmissions = _CidbContext.Missions.Include(m=>m.MissionMedia).Include(m=>m.MissionApplications).Include(m => m.City).Include(m => m.Theme).Include(m => m.FavouritMissions).Include(m => m.Country).Where(m => m.MissionId != mid && (m.City.Name == mission.City.Name || m.Theme.Title == mission.Theme.Title || m.Country.Name == mission.Country.Name) && m.Status==true && m.DeletedAt==null).Include(m=>m.MissionApplications).AsQueryable();
            //For adding only 3 missions
            List<Mission> limitedRelatedMissions = new List<Mission>();
            
            var countrelatedMission = 0;
            //Calling mission rating
            var mission_rating = _CidbContext.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            //Callig fav missions
            var fav_includings = _CidbContext.FavouritMissions.Include(m => m.Mission).Include(m => m.User).Where(m => m.User.UserId.ToString() == userid && m.MissionId == mid);
            //Documents
            var missiondocuments=_CidbContext.MissionDocuments.Include(m=>m.Mission).Where(m=>m.MissionId==mid).ToList();
            bool favchk;
            if (fav_includings.Count() == 0)
            {
                favchk = false;
            }
            else
            {
                favchk = true;
            }
            foreach (var i in relatedmissions)
            {
                if (countrelatedMission < 3)
                {
                    if (i.CityId == mission.CityId)
                    {
                        limitedRelatedMissions.Add(i);

                    }
                    else if (i.CountryId == mission.CountryId)
                    {
                        limitedRelatedMissions.Add(i);
                    }
                    else if (i.ThemeId == mission.ThemeId)
                    {
                        limitedRelatedMissions.Add(i);
                    }
                    countrelatedMission++;
                }
                else
                {
                    break;
                }
            }
            VolunteeringMissionViewModel volunteeringMission = new VolunteeringMissionViewModel()
            {
                goalMissions = _CidbContext.GoalMissions.ToList(),
                Timesheet = _CidbContext.Timesheets.ToList(),
                RelatedMissions = limitedRelatedMissions.ToList(),
                totalSeats = mission.TotalSeat,
                MissionId = mission.MissionId,
                Theme = mission.Theme.Title,
                City = mission.City.Name,
                CountryId = mission.CountryId,
                Title = mission.Title,
                ShortDescription = mission.ShortDescription,
                Description = mission.Description,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate,
                MissionType = mission.MissionType,
                Status = mission.Status,
                OrganizationName = mission.OrganizationName,
                OrganizationDetail = mission.OrganizationDetail,
                Availability = mission.Availability,
                SkillList = skills,
                MissionRatings = mission_rating,
                favouritMissions = favchk,
                ViewComments = comment_display,
                Missionapplications = missionapplications,
                missionDocuments= missiondocuments,
                deadlineDate = mission.DeadlineDate,
                missionMediaMediums = missionsMedia,
            };


            return volunteeringMission;
        }

        //Ratings
        public string updateandrate(int missionid, int rating, int userid)
        {
            var mission_rating = _CidbContext.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var rate_update = mission_rating.SingleOrDefault(m => m.User.UserId == userid && m.MissionId == missionid);
            if (userid == 0)
            {
                return "Failed";
            }
            if (rate_update != null)
            {
                var missionratings = new MissionRating
                {
                    MissionId = missionid,
                    UserId = userid,
                    Rating = rating,
                    MissionRatingId = rate_update.MissionRatingId,
                };
                rate_update.Rating = rating;
                _CidbContext.Update(rate_update);
                _CidbContext.SaveChanges();
            }
            if (rate_update == null)
            {
                var missionratings = new MissionRating
                {
                    MissionId = missionid,
                    UserId = userid,
                    Rating = rating,
                };

                _CidbContext.Add(missionratings);
                _CidbContext.SaveChanges();
            }
            return "successfull";
        }
        public bool chkfavourite(int userid, int missionid)
        {
            if (userid == 0)
            {
                return false;
            }
            else
            {
                var favchk = _CidbContext.FavouritMissions.Include(m => m.Mission).Include(m => m.User).FirstOrDefault(m => m.User.UserId == userid && m.MissionId == missionid);

                if (favchk == null)
                {
                    FavouritMission favourite = new FavouritMission()
                    {
                        UserId = userid,
                        MissionId = missionid,
                    };

                    _CidbContext.FavouritMissions.Add(favourite);
                }
                else
                {
                    _CidbContext.FavouritMissions.Remove(favchk);

                }
                _CidbContext.SaveChanges();
                return true;
            }
        }
        //Getting user for recommend to co-worker
        public List<User> GetUsersbyid(int Userid)
        {
            var user = _CidbContext.Users.Where(m => m.UserId != Userid && m.DeletedAt == null && m.Status == true).ToList();
            return user;
        }
        public string GetUsersId(int id, string url, int from_id, int mission_id)
        {
            int mid = id;

            var users = _CidbContext.Users.SingleOrDefault(m => m.UserId == mid);

            var from_user = _CidbContext.Users.SingleOrDefault(u => u.UserId == from_id);
            var missiontitle = _CidbContext.Missions.SingleOrDefault(m => m.MissionId == mission_id).Title;

            var from_email = new MailAddress("makhijashish2000@gmail.com", "Ashish");
            // jnptyjhdvcbbrlpp
            var to_email = new MailAddress(users.Email);
            var subject = "Recommeneded Volunteer Page by " + users.FirstName;
            var body = $"Hi,<br /><br />Please click on the following to apply on the recommeneded mission:<br /><br /><a href='{url}'>{url}</a>";
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
            var missioninvite = new MissionInvite
            {
                MissionId = mission_id,
                FromUserId = from_id,
                ToUserId = id
            };
            _CidbContext.Add(missioninvite);
            _CidbContext.SaveChanges();

            //For Notification
            var enable_check = _CidbContext.EnableUserStatuses.SingleOrDefault(e => e.UserId == id && e.NotificationId == 1);

            if (enable_check == null && enable_check.Status == 1)
            {
                var messagemodel = new MessageTable
                {
                    NotificationId = 1,
                    Message = $"{from_user.FirstName}-Recommended Mission-{missiontitle}",
                    Url = $"https://localhost:44340/Mission/volunteerMission/{mission_id}",
                    AvatarUser = from_user.Avatar
                };
                _CidbContext.Add(messagemodel);
                var userpermission = new Userpermission
                {
                    UserId = id
                };
                messagemodel.Userpermissions.Add(userpermission);
            }
            _CidbContext.SaveChanges();
            return "Successful";
        }
        public string UserCommentPost(string commentstext, int MissionId, int userId)
        {
            var comment = new Comment
            {
                MissionId = MissionId,
                UserId = userId,
                ComentDescription = commentstext,
                ApprovalStatus = true,
                CreatedAt = DateTime.Now,
            };
            _CidbContext.Add(comment);
            _CidbContext.SaveChanges();
            return "success";
        }
        public string applymission(string userid, int MissionId)
        {
            if (userid == null)
            {
                return "Pleaselogin";

            }
            else
            {
                var appliedmissions = _CidbContext.MissionApplications.Include(m => m.Mission).Include(m => m.User).FirstOrDefault(m => m.User.UserId.ToString() == userid && m.MissionId == MissionId);
                if(appliedmissions == null)
                {
                    MissionApplication missionApplication = new MissionApplication
                    {
                        UserId = int.Parse(userid),
                        MissionId = MissionId,
                        ApprovalStatus="pending",
                        AppliedAt = DateTime.Now,
                        
                    };
                    _CidbContext.MissionApplications.Add(missionApplication);
                    _CidbContext.SaveChanges();
                    return "AppliedSuccessfully";
                }
                else
                {
                    return "AlreadyApplied";
                }
               
            }
        }
    }
}
