using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Repository
{
    public class UserRepository : IUserInterface
    {
        /*private readonly IRepository _repository;*/
        private readonly CidbContext _CidbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserRepository(CidbContext cidbContext, IWebHostEnvironment webHostEnvironment)
        {
            _CidbContext = cidbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public CMSPageForAdminViewModel cmsData()
        {
            var cms = new CMSPageForAdminViewModel();
            /*cms.cmspage = _CidbContext.CmsPages.Where(c=>c.DeletedAt == null && c.Status == false).ToList();*/
            return cms;
        }

        public UsersViewModel GetBanner()
        {
            var banner = new UsersViewModel();
            banner.bannerList = _CidbContext.Banners.Where(b => b.DeletedAt == null && b.Status == true).OrderBy(m=>m.SortOrder).ToList();
            return banner;
        }  
      
        public RegisterationViewModel RGetBanner()
        {
            var banner = new RegisterationViewModel();
            banner.bannerList = _CidbContext.Banners.Where(b => b.DeletedAt == null && b.Status == true).OrderBy(m=>m.SortOrder).ToList();
            return banner;
        }
        public LostpasViewModel LGetBanner()
        {
            var banner = new LostpasViewModel();
            banner.bannerList = _CidbContext.Banners.Where(b => b.DeletedAt == null && b.Status == true).OrderBy(m => m.SortOrder).ToList();
            return banner;
        }


        public int Index(User userdetails)
        {
            var chk = _CidbContext.Users.Where(model => model.Email == userdetails.Email && model.Password == userdetails.Password).FirstOrDefault();
            var emchk = _CidbContext.Users.Where(model => model.Email == userdetails.Email).FirstOrDefault();
            var adminEmailchk=_CidbContext.Admins.Where(a=>a.Email == userdetails.Email).FirstOrDefault();
            var adminchk = _CidbContext.Admins.Where(model => model.Email == userdetails.Email && model.Password == userdetails.Password).FirstOrDefault();
            if (emchk == null && adminEmailchk == null)
            {
                return -1;
            }

            else if (chk == null && adminchk==null)
            {
                return 0;
            }
            else
                return 1;
        }



        public int Register(RegisterationViewModel user)
        {
            var UserExists = _CidbContext.Users.Where(model => model.Email == user.Email).FirstOrDefault();
            var AdminExists =  _CidbContext.Admins.Where(m=>m.Email == user.Email).FirstOrDefault();
            if (UserExists != null || AdminExists != null)
            {
                return 0;
            }
            else
            {
                User _user = new User()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    SecondName = user.LastName,
                    Password = user.NewPassword,
                    PhoneNumber = user.PhoneNumber,
                    CityId = 1,
                    CountryId = 1
                };
                _CidbContext.Users.Add(_user);
                _CidbContext.SaveChangesAsync();
                return 1;

            }
        }
        public int lostpas(User user)
        {
            var UserExists = _CidbContext.Users.Where(model => model.Email == user.Email).FirstOrDefault();
            if (UserExists != null)
            {
                return 0;
            }
            else
            {
                User _user = new User()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    CityId = 1,
                    CountryId = 1
                };
                _CidbContext.Users.Add(_user);
                _CidbContext.SaveChangesAsync();
                return 1;

            }
        }
        public UserDetailsViewModel Getuserdata(string userid)
        {
            var UserInfo = _CidbContext.Users.Include(u=>u.Country).Include(u=>u.City).Where(user => user.UserId.ToString() == userid).FirstOrDefault();
            var userdetails = new UserDetailsViewModel();
            userdetails.FirstName = UserInfo.FirstName;
            userdetails.SecondName = UserInfo.SecondName;
            userdetails.CityName = UserInfo.City.Name;
            userdetails.CountryName = UserInfo.Country.Name;
            userdetails.LinkedInUrl = UserInfo.LinkedInUrl;
            userdetails.ProfileText = UserInfo.ProfileText;
            userdetails.WhyIVolunteer = UserInfo.WhyIVolunteer;
            userdetails.Department = UserInfo.Department;
            userdetails.Title = UserInfo.Title;
            userdetails.Availability = UserInfo.Availability;
            userdetails.EmployeeId = UserInfo.EmployeeId;
            userdetails.Countryid = UserInfo.CountryId;
            userdetails.Cityid = UserInfo.CityId;
            userdetails.UserId=int.Parse(userid);
            userdetails.Manager=UserInfo.Manager;
            userdetails.Avatar = UserInfo.Avatar;
           
            return userdetails;
        }
        public void SaveUserData(UserDetailsViewModel userDetailsViewModel, string userid)
        {
            var user = _CidbContext.Users.Where(u=>u.UserId.ToString() == userid).FirstOrDefault();
            user.FirstName = userDetailsViewModel.FirstName;
            user.SecondName = userDetailsViewModel.SecondName;
           /* user.CityName = UserInfo.City.Name;
            user.CountryName = UserInfo.Country.Name;*/
            user.LinkedInUrl = userDetailsViewModel.LinkedInUrl;
            user.ProfileText = userDetailsViewModel.ProfileText;
            user.WhyIVolunteer = userDetailsViewModel.WhyIVolunteer;
            user.Department = userDetailsViewModel.Department;
            user.Manager = userDetailsViewModel.Manager;
            user.Title = userDetailsViewModel.Title;
            user.Availability = userDetailsViewModel.Availability;
            user.EmployeeId = userDetailsViewModel.EmployeeId;
            user.CountryId = userDetailsViewModel.Countryid;
            user.CityId = userDetailsViewModel.Cityid;
            user.Status = true;
            var check = _CidbContext.UserSkills.Where(s => s.UserId.ToString() == userid).ToList();
            if (check == null)
            {
                foreach (var skills in userDetailsViewModel.userselectedSkills)
                {
                    var skillmodal = new UserSkill
                    {
                        UserId = int.Parse(userid),
                        SkillId = skills,
                    };
                    _CidbContext.Add(skillmodal);
                }
            }
            else
            {
                var removeskills = _CidbContext.UserSkills.Where(s => s.UserId.ToString() == userid);
                _CidbContext.UserSkills.RemoveRange(removeskills);
                if(userDetailsViewModel.userselectedSkills != null)
                {
                    foreach (var skills in userDetailsViewModel.userselectedSkills)
                    {
                        var skillmodal = new UserSkill
                        {
                            UserId = int.Parse(userid),
                            SkillId = skills,
                        };
                        _CidbContext.Add(skillmodal);
                    }
                }
                
            }
            _CidbContext.SaveChanges();

        }
        public List<Country> GetCountryList(int cid)
        {
            return _CidbContext.Countries.Where(c=>c.CountryId!=cid).OrderBy(c=>c.Name).ToList();
        } 
        public List<City> GetCityList(int id, int cityid)
        {
            var city = _CidbContext.Cities.Where(c => c.CountryId == id && c.CityId!=cityid).OrderBy(c=>c.Name).ToList();
            return city;
        }
        public UserDetailsViewModel getSkill(string userid)
        {
            var userdetails = new UserDetailsViewModel();
            userdetails.userSkill= _CidbContext.UserSkills.Include(s=>s.Skill).Where(s => s.UserId.ToString() == userid).ToList();
            userdetails.Skills = _CidbContext.Skills.OrderBy(s=>s.SkillName).ToList();
            return userdetails;
        }

        public string UpdatePassword(UserDetailsViewModel userDetailsViewModel, string userid)
        {
            var passwordchk = _CidbContext.Users.Where(m=>m.UserId.ToString() == userid && m.Password==userDetailsViewModel.Password).FirstOrDefault();
            if(passwordchk!=null)
            {
                passwordchk.Password = userDetailsViewModel.NewPassword;
                _CidbContext.SaveChanges();

                return "Updated";
            }
            else
            {
                return "Incorrect";
            }
        }
        public void ContactUs(string userid, ContactUsViewModel contactUsViewModel)
        {
            var usercontacted = new ContactU
            {
                Userid = int.Parse(userid),
                Message = contactUsViewModel.Message,
                Subject= contactUsViewModel.Subject,
            };
            _CidbContext.Add(usercontacted);

            _CidbContext.SaveChanges();

        }
        public void editImage(IFormFile image,int userid)
        {
            var user=_CidbContext.Users.Where(u=>u.UserId == userid).FirstOrDefault();
            string wwwpath = _webHostEnvironment.WebRootPath;
            string fileName=Guid.NewGuid().ToString();
            var uploaded = Path.Combine(wwwpath, @"documents");
            var extension = Path.GetExtension(image.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploaded, fileName + extension), FileMode.Create))
            {
                image.CopyTo(fileStreams);
            }
            user.Avatar = @"\documents\" + fileName + extension;
            _CidbContext.SaveChanges();

        }
        public List<Admin> AdminChk(User userdetails)
        {
            var adminch=_CidbContext.Admins.Where(admin=>admin.Email==userdetails.Email && admin.Password==userdetails.Password).ToList();
            if(adminch!=null)
            {
                return adminch;
            }
            return adminch;
        }


        //Notification
        public Tuple<List<NotificationTitle>, List<long>> gettitles(string userId)
        {
            var notificationTitle = _CidbContext.NotificationTitles.ToList();
            List<long> idsselected = _CidbContext.EnableUserStatuses.Where(up => up.UserId.ToString() == userId && up.Status == 1).Select(up => up.NotificationId).ToList().ConvertAll(id => (long?)id).Select(id => id.Value).ToList();
            return new Tuple<List<NotificationTitle>, List<long>>(notificationTitle, idsselected);
        }

        public void setstatus(string userid, List<string> titles)
        {
            var enables = _CidbContext.EnableUserStatuses.Where(e => e.UserId.ToString() == userid).ToList();
            _CidbContext.EnableUserStatuses.RemoveRange(enables);
            foreach (var id in titles)
            {
                var model = new EnableUserStatus
                {
                    UserId = long.Parse(userid),
                    Status = 1,
                    NotificationId = long.Parse(id),
                };
                _CidbContext.Add(model);
            }
            _CidbContext.SaveChanges();
        }
        public void changestatus(int messageid, string userid)
        {
            var userrecord = _CidbContext.Userpermissions.SingleOrDefault(up => up.MessageId == messageid && up.UserId.ToString() == userid);
            userrecord.Seen = 0;
            _CidbContext.SaveChanges();
        }
        public List<Tuple<string, long, string, string, int, int, string>> getnotification(string userId)
        {
            var notifications = new List<Tuple<string, long, string, string, int, int, string>>();
            var takeids = _CidbContext.EnableUserStatuses.Where(e => e.UserId.ToString() == userId).Select(e => e.NotificationId).ToList();
            foreach (var id in takeids)
            {
                var message = _CidbContext.MessageTables.Where(m => m.NotificationId == id).AsQueryable();
                var messageid = message.Select(m => m.MessageId).ToList();
                foreach (var id1 in messageid)
                {
                    var check_status = _CidbContext.Userpermissions.SingleOrDefault(u => u.UserId.ToString() == userId && u.MessageId == id1);
                    if (check_status != null && check_status.Status == 1)
                    {
                        var messages = message.FirstOrDefault(m => m.MessageId == id1);
                        DateTime createdAt = (DateTime)messages.CreatedAt;
                        notifications.Add(Tuple.Create(messages.Message, (long)messages.NotificationId, createdAt.ToString("d MMM, H:mm"), messages.Url, messages.MessageId, check_status.Seen, messages.AvatarUser));
                    }
                }
            }
            return notifications;
        }

    }
}
