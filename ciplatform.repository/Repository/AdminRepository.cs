using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using X.PagedList;


namespace ciplatform.repository.Repository
{
    public class AdminRepository : IAdminInterface
    {
        private readonly CidbContext _CidbContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        //For md5
        private readonly IConfiguration _configuration;
        private string securityKey;


        public AdminRepository(CidbContext cidbContext, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _CidbContext = cidbContext;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            this.securityKey = _configuration.GetValue<string>("SecurityKey");


        }
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public IPagedList<UserPageForAdminViewModel> getUsers(string searchkeyword, int pageIndex)
        {
            var Userdata = _CidbContext.Users.Where(m => (searchkeyword == null || m.FirstName.Contains(searchkeyword) || m.SecondName.Contains(searchkeyword) || m.Department.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
            var UsersDeatils = new List<UserPageForAdminViewModel>();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();



            foreach (var user in Userdata)
            {
                //Generating QRcode
                QRCodeData QrCodeInfo = qrGenerator.CreateQrCode("https://localhost:7246/Admin/EditUser/" + user.UserId, QRCodeGenerator.ECCLevel.Q);

                QRCode qrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = qrCode.GetGraphic(60);
                byte[] BitmapArray = BitmapToByteArray(QrBitmap);
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                var EachUser = new UserPageForAdminViewModel
                {
                    QRUri = QrUri,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Department = user.Department,
                    EmployeeId = user.EmployeeId,
                    UserId = user.UserId,
                    Status = user.Status,
                    Email = user.Email,

                };
                UsersDeatils.Add(EachUser);
            }
            return UsersDeatils.ToPagedList(pageIndex, 9);
        }
        public int RemoveUser(int userid)
        {
            var removeuser = _CidbContext.Users.Where(u => u.UserId == userid).FirstOrDefault();
            if (removeuser != null)
            {
                removeuser.DeletedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;

            }
            return 0;
        }
        public int AddUser(UserPageForAdminViewModel AdduserPageForAdminView, IFormFileCollection formFilesImg)
        {
            if (_CidbContext.Users.Where(u => u.Email == AdduserPageForAdminView.Email).Count() == 0)
            {
                var UserData = new User
                {
                    FirstName = AdduserPageForAdminView.FirstName,
                    SecondName = AdduserPageForAdminView.SecondName,
                    Password = AdduserPageForAdminView.Password,
                    Email = AdduserPageForAdminView.Email,
                    CityId = AdduserPageForAdminView.Cityid,
                    CountryId = AdduserPageForAdminView.Countryid,
                    Status = AdduserPageForAdminView.Status,
                    PhoneNumber = AdduserPageForAdminView.PhoneNumber
                };
                if (formFilesImg.Count > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(formFilesImg[0].FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedImages", ImageName);

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        UserData.Avatar = "/UploadedImages/" + ImageName;
                        formFilesImg[0].CopyTo(stream);
                    }
                }
                _CidbContext.Add(UserData);
                _CidbContext.SaveChanges();
                return 1;
            }
            else
                return 0;
        }
        public void EditUser(UserPageForAdminViewModel AdduserPageForAdminView, IFormFileCollection formFilesImg)
        {
            var EditUserData = _CidbContext.Users.Where(u => u.UserId == AdduserPageForAdminView.UserId).FirstOrDefault();

            EditUserData.FirstName = AdduserPageForAdminView.FirstName;
            EditUserData.SecondName = AdduserPageForAdminView.SecondName;
            EditUserData.Password = AdduserPageForAdminView.Password;
            EditUserData.Email = AdduserPageForAdminView.Email;
            EditUserData.CityId = AdduserPageForAdminView.Cityid;
            EditUserData.EmployeeId = AdduserPageForAdminView.EmployeeId;
            EditUserData.CountryId = AdduserPageForAdminView.Countryid;
            EditUserData.Status = AdduserPageForAdminView.Status;
            EditUserData.PhoneNumber = AdduserPageForAdminView.PhoneNumber;
            EditUserData.UpdatedAt = DateTime.Now;
            if (formFilesImg.Count > 0)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(formFilesImg[0].FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedImages", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    EditUserData.Avatar = "/UploadedImages/" + ImageName;
                    formFilesImg[0].CopyTo(stream);
                }
            }

            _CidbContext.SaveChanges();
        }
        public UserPageForAdminViewModel GetUserDetail(int id)
        {
            var userData = _CidbContext.Users.Include(m => m.Country).Include(m => m.City).Where(m => m.UserId == id).FirstOrDefault();
            var UserDetail = new UserPageForAdminViewModel();
            UserDetail.FirstName = userData.FirstName;
            UserDetail.SecondName = userData.SecondName;
            UserDetail.Email = userData.Email;
            UserDetail.ProfileText = userData.ProfileText;
            UserDetail.Status = userData.Status;
            UserDetail.EmployeeId = userData.EmployeeId;
            UserDetail.Countryid = userData.CountryId;
            UserDetail.Cityid = userData.CityId;
            UserDetail.PhoneNumber = (int)userData.PhoneNumber;
            UserDetail.Password = userData.Password;
            UserDetail.UserId = userData.UserId;
            UserDetail.Avatar = userData.Avatar;
            UserDetail.Countryname = userData.Country.Name;
            UserDetail.CityName = userData.City.Name;
            return UserDetail;

        }


        //Missions
        public IPagedList<MissionPageForAdminViewModel> getMissions(string searchkeyword, int pageIndex)
        {

            var Missiondata = _CidbContext.Missions.Where(m => (searchkeyword == null || m.Title.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
            var MissionsDeatils = new List<MissionPageForAdminViewModel>();
            foreach (var mission in Missiondata)
            {
                var EachMission = new MissionPageForAdminViewModel
                {
                    Title = mission.Title,
                    MissionId = mission.MissionId,
                    StartDate = mission.StartDate,
                    EndDate = mission.EndDate,
                    MissionType = mission.MissionType,
                    Status = mission.Status,

                };
                MissionsDeatils.Add(EachMission);
            }


            return MissionsDeatils.ToPagedList(pageIndex, 9);
        }
        public int RemoveMission(int Missionid)
        {
            var removemission = _CidbContext.Missions.Where(u => u.MissionId == Missionid).FirstOrDefault();
            if (removemission != null)
            {
                removemission.DeletedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;

            }
            return 0;
        }


        // Add and Edit Missions

        public MissionPageForAdminViewModel getmissionmodeldata()
        {
            var skills = _CidbContext.Skills.ToList();
            var model = new MissionPageForAdminViewModel
            {
                Skills = skills
            };
            return model;
        }
        public void Addmission(MissionPageForAdminViewModel model, List<int> selectedSkills)
        {
            var model1 = new Mission
            {
                Title = model.Title,
                CityId = model.CityId,
                CountryId = model.CountryId,
                OrganizationDetail = model.OrganizationDetail,
                OrganizationName = model.OrganizationName,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                StartDate = model.StartDate,
                Availability = model.Availability,
                TotalSeat = model.TotalSeats,
                ThemeId = model.ThemeId,
                Status = model.Status,
                MissionType = model.MissionType,
                EndDate = model.EndDate,
                DeadlineDate = model.RegistrationDeadline,

            };


            _CidbContext.Add(model1);

            foreach (var skill in selectedSkills)
            {
                var model3 = new MissionSkill
                {
                    SkillId = skill,
                };
                model1.MissionSkills.Add(model3);
            }

            if (model.MissionType == false)
            {
                var model2 = new GoalMission
                {
                    GoalObjectiveText = model.GoalObjectiveText,
                    GoalValue = model.GoalValue,

                };
                model1.GoalMissions.Add(model2);
            }

            _CidbContext.SaveChanges();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
            string MainfolderPath = Path.Combine(imagesFolderPath, "Missions");
            if (!Directory.Exists(MainfolderPath))
            {
                Directory.CreateDirectory(MainfolderPath);
            }
            //string folderName = model.MissionTitle;
            //string folderPath = Path.Combine(MainfolderPath, folderName);
            var mission = _CidbContext.Missions.FirstOrDefault(m => m.MissionId == model1.MissionId);
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            foreach (var Image in model.Images)
            {
                string fileName = Image.FileName;
                var uploads = Path.Combine(MainfolderPath, fileName);
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    Image.CopyTo(fileStreams);
                }
                var viewModel = new MissionMedium
                {
                    MissionId = mission.MissionId,
                    MediaName = fileName,
                    MediaType = ".png",
                    MediaPath = @"\images\Missions\",
                };
                _CidbContext.Add(viewModel);
                _CidbContext.SaveChanges();
            }

            string documentFolderPath = Path.Combine(wwwRootPath, "documents");
            //string missiondocfolderPath = Path.Combine(missiondocPath, folderName);
            //if (!Directory.Exists(missiondocfolderPath))
            //{
            //    Directory.CreateDirectory(missiondocfolderPath);
            //}
            foreach (var doc in model.Documents)
            {
                string fileName = doc.FileName;
                var uploads = Path.Combine(documentFolderPath, fileName);
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    doc.CopyTo(fileStreams);
                }
                MissionDocument docModel = new MissionDocument()
                {
                    MissionId = mission.MissionId,
                    DocumentName = doc.FileName,
                    DocumentPath = @"\documents\",
                };

                switch (Path.GetExtension(doc.FileName))
                {
                    case ".doc":
                    case ".docx":
                        docModel.DocumentType = "docx";
                        break;
                    case ".xls":
                    case ".xlsx":
                        docModel.DocumentType = "xlsx";
                        break;
                    case ".pdf":
                        docModel.DocumentType = "pdf";
                        break;
                    default:
                        // Handle other types of documents here
                        break;
                }
                _CidbContext.MissionDocuments.Add(docModel);
            }
            var message = new MessageTable
            {
                NotificationId = 5,
                Message = $"New Mission-{model.Title}",
                Url = $"https://localhost:44381/LandingPage/VolunteeringMission/{model1.MissionId}",
            };
            _CidbContext.Add(message);
            var users = _CidbContext.EnableUserStatuses.Where(e => e.NotificationId == 5).Select(e => e.UserId).ToList();
            foreach (var userId in users)
            {
                var userpermission = new Userpermission
                {
                    UserId = userId,

                };
                message.Userpermissions.Add(userpermission);
            }
            _CidbContext.SaveChanges();

        }

        public MissionPageForAdminViewModel editmissondata(string missonid)
        {
            var mission = _CidbContext.Missions.FirstOrDefault(m => m.MissionId.ToString() == missonid);
            var goalmission = _CidbContext.GoalMissions.Include(g => g.Mission).FirstOrDefault(g => g.MissionId.ToString() == missonid);

            List<SelectListItem> list = new List<SelectListItem>();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var temp = _CidbContext.Countries.ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _CidbContext.Cities.Where(c => c.CountryId == mission.CountryId).ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            List<SelectListItem> list2 = new List<SelectListItem>();
            var temp2 = _CidbContext.Themes.ToList();
            foreach (var item in temp2)
            {
                list2.Add(new SelectListItem() { Text = item.Title, Value = item.ThemeId.ToString() });
            }
            var skills = _CidbContext.Skills.ToList();
            var skillids_mission = _CidbContext.MissionSkills.Include(m => m.Skill).Where(m => m.MissionId == mission.MissionId).Select(m => m.SkillId).ToList();

            var model = new MissionPageForAdminViewModel
            {
                countries = list,
                cities = list1,
                Themes = list2,
                Title = mission.Title,
                CountryId = mission.CountryId,
                CityId = mission.CityId,
                ThemeId = mission.ThemeId,
                Description = mission.Description,
                ShortDescription = mission.ShortDescription,
                TotalSeats = mission.TotalSeat,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate,
                Status = mission.Status,
                MissionType = mission.MissionType,
                OrganizationDetail = mission.OrganizationDetail,
                OrganizationName = mission.OrganizationName,
                Availability = mission.Availability,
                MissionId = mission.MissionId,
                skillids = skillids_mission,
                Skills = skills,
                RegistrationDeadline = mission.DeadlineDate,

            };

            List<MissionMedium> missionMedia = _CidbContext.MissionMedia.Where(m => m.MissionId.ToString() == missonid).ToList();
            List<MissionDocument> missionDoc = _CidbContext.MissionDocuments.Where(m => m.MissionId.ToString() == missonid).ToList();
            List<IFormFile> imageFiles = new();
            List<IFormFile> docFiles = new();



            foreach (var m in missionMedia)
            {
                string fullPath = wwwRootPath + m.MediaPath + m.MediaName;
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    IFormFile file = new FormFile(stream, 0, new FileInfo(fullPath).Length, null, Path.GetFileName(fullPath));
                    imageFiles.Add(file);
                }
            }

            foreach (var m in missionDoc)
            {
                string fullPath = wwwRootPath + m.DocumentPath + m.DocumentName;
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    IFormFile file = new FormFile(stream, 0, new FileInfo(fullPath).Length, null, Path.GetFileName(fullPath));
                    docFiles.Add(file);
                }
            }
            model.Images = imageFiles;
            model.Documents = docFiles;
            if (model.MissionType == false)
            {
                model.GoalObjectiveText = goalmission.GoalObjectiveText;
                model.GoalValue = goalmission.GoalValue;
            };
            return model;
        }

        public void Editmission(MissionPageForAdminViewModel model, List<int> selectedSkills)
        {
            var mission = _CidbContext.Missions.SingleOrDefault(m => m.MissionId == model.MissionId);
            List<MissionMedium> missionMedia = _CidbContext.MissionMedia.Where(m => m.MissionId == model.MissionId).ToList();
            List<MissionDocument> missionDoc = _CidbContext.MissionDocuments.Where(m => m.MissionId == model.MissionId).ToList();
            var missionskills = _CidbContext.MissionSkills.Where(m => m.MissionId == model.MissionId);
            _CidbContext.MissionSkills.RemoveRange(missionskills);
            var goals = _CidbContext.GoalMissions.Where(g => g.MissionId == model.MissionId).ToList();
            if (model.MissionType == false)
            {
                _CidbContext.GoalMissions.RemoveRange(goals);
                var goalmodel = new GoalMission
                {
                    GoalObjectiveText = model.GoalObjectiveText,
                    GoalValue = model.GoalValue
                };
                mission.GoalMissions.Add(goalmodel);
            }
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (model.Title != mission.Title)
            {

                string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
                string MainfolderPath = Path.Combine(imagesFolderPath, "Missions");
                //string oldfolderpath = Path.Combine(MainfolderPath, mission.Title);
                //string folderName = model.MissionTitle;
                //string newfolderPath = Path.Combine(MainfolderPath, folderName);
                //if (!Directory.Exists(newfolderPath))
                //{
                //    Directory.CreateDirectory(newfolderPath);
                //}
                string[] imageFiles = Directory.GetFiles(MainfolderPath, "*.png|*.jpg");
                foreach (string imageFile in imageFiles)
                {
                    string fileName = Path.GetFileName(imageFile);
                    string destFile = Path.Combine(MainfolderPath, fileName);
                    File.Copy(imageFile, destFile, true);
                }


                string documentFolderPath = Path.Combine(wwwRootPath, "Documents");
                //string docoldfolderpath = Path.Combine(docmainfolderPath, mission.Title);
                //string docnewfolderPath = Path.Combine(docmainfolderPath, folderName);
                //if (!Directory.Exists(docnewfolderPath))
                //{
                //    Directory.CreateDirectory(docnewfolderPath);
                //}
                string[] docFiles = Directory.GetFiles(documentFolderPath, "*.doc|*.pdf|*.xlsx|*.xls");
                foreach (string docFile in docFiles)
                {
                    string fileName = Path.GetFileName(docFile);
                    string destFile = Path.Combine(documentFolderPath, fileName);
                    File.Copy(docFile, destFile, true);
                }



            }
            foreach (var skill in selectedSkills)
            {
                MissionSkill skillmodel = new MissionSkill
                {
                    SkillId = skill,
                };
                mission.MissionSkills.Add(skillmodel);
            }
            if (model.Images != null)
            {
                if (missionMedia.Count() != 0)
                {
                    _CidbContext.MissionMedia.RemoveRange(missionMedia);
                }

                string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
                string MainfolderPath = Path.Combine(imagesFolderPath, "Missions");
                //if (!Directory.Exists(MainfolderPath))
                //{
                //    Directory.CreateDirectory(MainfolderPath);
                //}
                //string folderName = model.MissionTitle;
                //string folderPath = Path.Combine(MainfolderPath, folderName);
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}

                foreach (var Image in model.Images)
                {
                    string fileName = Image.FileName;
                    var uploads = Path.Combine(MainfolderPath, fileName);
                    using (var fileStreams = new FileStream(uploads, FileMode.Create))
                    {
                        Image.CopyTo(fileStreams);
                    }
                    var viewModel = new MissionMedium
                    {
                        MediaName = fileName,
                        MediaType = ".png",
                        MediaPath = @"\Images\Missions\",
                    };
                    mission.MissionMedia.Add(viewModel);
                }
            }
            if (model.Documents != null)
            {
                if (missionDoc.Count() != 0)
                {
                    _CidbContext.MissionDocuments.RemoveRange(missionDoc);
                }

                string docFolderPath = Path.Combine(wwwRootPath, "Documents");
                //if (!Directory.Exists(docMainfolderPath))
                //{
                //    Directory.CreateDirectory(docMainfolderPath);
                //}
                //string folderName = model.MissionTitle;
                //string docfolderPath = Path.Combine(docMainfolderPath, folderName);
                //if (!Directory.Exists(docfolderPath))
                //{
                //    Directory.CreateDirectory(docfolderPath);
                //}
                foreach (var doc in model.Documents)
                {
                    string fileName = doc.FileName;
                    var uploads = Path.Combine(docFolderPath, fileName);
                    using (var fileStreams = new FileStream(uploads, FileMode.Create))
                    {
                        doc.CopyTo(fileStreams);
                    }
                    MissionDocument docModel = new MissionDocument()
                    {
                        DocumentName = doc.FileName,
                        DocumentPath = @"\documents\" + fileName,
                    };

                    switch (Path.GetExtension(doc.FileName))
                    {
                        case ".doc":
                        case ".docx":
                            docModel.DocumentType = "docs";
                            break;
                        case ".xls":
                        case ".xlsx":
                            docModel.DocumentType = "xlsx";
                            break;
                        case ".pdf":
                            docModel.DocumentType = "pdf";
                            break;
                        default:
                            break;
                    }
                    mission.MissionDocuments.Add(docModel);
                }
            }
            mission.Title = model.Title;
            mission.CityId = model.CityId;
            mission.CountryId = model.CountryId;
            mission.OrganizationDetail = model.OrganizationDetail;
            mission.OrganizationName = model.OrganizationName;
            mission.ShortDescription = model.ShortDescription;
            mission.Description = model.Description;
            mission.StartDate = model.StartDate;
            mission.EndDate = model.EndDate;
            mission.TotalSeat = model.TotalSeats;
            mission.Availability = model.Availability;
            mission.ThemeId = model.ThemeId;
            mission.MissionType = model.MissionType;
            mission.Status = model.Status;
            mission.DeadlineDate = model.RegistrationDeadline;
            _CidbContext.SaveChanges();
        }


        public List<Theme> getthemes()
        {
            var themes = _CidbContext.Themes.Where(t => t.DeletedAt == null && t.Status == 1).ToList();
            return themes;
        }


        //Mission Applications
        public IPagedList<MissionApplicationForAdminViewModel> getApplicationsOfMission(string searchkeyword, int pageIndex)
        {
            var Missiondata = _CidbContext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => (searchkeyword == null || m.Mission.Title.Contains(searchkeyword) || m.User.FirstName.Contains(searchkeyword) || m.User.SecondName.Contains(searchkeyword) || m.Mission.MissionId.ToString().Contains(searchkeyword) || m.UserId.ToString().Contains(searchkeyword) || m.AppliedAt.ToString().Contains(searchkeyword)) && m.DeletedAt == null && m.ApprovalStatus == "pending").ToList();
            var MissionsDeatils = new List<MissionApplicationForAdminViewModel>();
            foreach (var mission in Missiondata)
            {
                var EachMissionApplication = new MissionApplicationForAdminViewModel
                {
                    MissionTitle = mission.Mission.Title,
                    MissionId = mission.Mission.MissionId,
                    UserId = mission.UserId,
                    UserName = mission.User.FirstName + " " + mission.User.SecondName,
                    AppliedAt = mission.AppliedAt,
                    ApprovalStatus = mission.ApprovalStatus,
                    MissionApplicationId = mission.MissionApplicationId,

                };
                MissionsDeatils.Add(EachMissionApplication);
            }


            return MissionsDeatils.ToPagedList(pageIndex, 9);
        }
        public void ChangeApplicationStatus(int MissionApplicationId, string approvalstatus)
        {
            var UpdateApproval = _CidbContext.MissionApplications.Where(applcation => applcation.MissionApplicationId == MissionApplicationId).FirstOrDefault();
            if (UpdateApproval != null)
            {
                UpdateApproval.ApprovalStatus = approvalstatus;
                UpdateApproval.UpdatedAt = DateTime.Now;
                _CidbContext.SaveChanges();

            }

        }

        public IPagedList<StoryPageForAdminViewModel> getStories(string searchkeyword, int pageIndex, int min, int max)
        {
            // status = 2 means status is pending, 1 means accepted 3 means rejected
            var Storydata = _CidbContext.Stories.Include(m => m.StoryMedia).Include(m => m.Mission).Include(m => m.User).Where(m => (searchkeyword == null || m.Title.Contains(searchkeyword) || m.Mission.Title.Contains(searchkeyword) || m.User.FirstName.Contains(searchkeyword) || m.User.SecondName.Contains(searchkeyword)) && m.Viewcount >= min && m.Viewcount <= max && m.DeletedAt == null).ToList();
            //&& m.Status == 2
            var StoryDetails = new List<StoryPageForAdminViewModel>();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            //MD5 Encryption
            byte[] keyArray;
            byte[] keyArrayD;



            foreach (var story in Storydata)
            {
                QRCodeData QrCodeInfo = qrGenerator.CreateQrCode("https://localhost:7246/StoryListing/storydetail?sid=" + story.StoryId, QRCodeGenerator.ECCLevel.Q);

                QRCode qrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = qrCode.GetGraphic(60);
                byte[] BitmapArray = BitmapToByteArray(QrBitmap);
                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                string key = securityKey;

                var EachStory = new StoryPageForAdminViewModel
                {
                    QRUri = QrUri,
                    StoryId = story.StoryId,
                    StoryTitle = story.Title,
                    MissionId = story.MissionId,
                    MissionTitle = story.Mission.Title,
                    UserName = story.User.FirstName + ' ' + story.User.SecondName,
                    Status = story.Status,
                    viewCount = (int)story.Viewcount,
                    Imgpath = story.StoryMedia.Where(m => m.StoryId == story.StoryId).FirstOrDefault().Path.ToString()


                };
                //Encryption
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
/*                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(EachStory.viewCount.ToString());
*/                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(EachStory.MissionTitle);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                //PKCS7 adds 1 byte for 1 space
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();


                /* EachStory.MissionTitle
                 For string*/
                EachStory.MissionTitle = Convert.ToBase64String(resultArray, 0, resultArray.Length);

                /*                For Converting into int form
                */
                /*EachStory.viewCount = BitConverter.ToInt32(resultArray, 0);
*/

                /*
                 * Without key directly MD5
                  var e=  string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(EachStory.viewCount.ToString())).Select(s => s.ToString("x2")));
                */



                /*Decryption*/
                byte[] toDecryptArray = Convert.FromBase64String(EachStory.MissionTitle);
                MD5CryptoServiceProvider Dhashmd5 = new MD5CryptoServiceProvider();

                keyArrayD = Dhashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                Dhashmd5.Clear();
                TripleDESCryptoServiceProvider tdesD = new TripleDESCryptoServiceProvider();

                tdesD.Key = keyArrayD;
                tdesD.Mode = CipherMode.ECB;
                tdesD.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransformDec = tdesD.CreateDecryptor();

                tdesD.Clear();
                byte[] resultArray2 = cTransformDec.TransformFinalBlock(
                                     toDecryptArray, 0, toDecryptArray.Length);

                var decr = UTF8Encoding.UTF8.GetString(resultArray2);


                StoryDetails.Add(EachStory);
            }


            return StoryDetails.ToPagedList(pageIndex, 50);

        }

        public void ChangeStoryStatus(int StoryId, int approvalstatus)
        {
            var UpdateApproval = _CidbContext.Stories.Where(story => story.StoryId == StoryId).FirstOrDefault();
            if (UpdateApproval != null)
            {
                UpdateApproval.Status = approvalstatus;
                UpdateApproval.UpdatedAt = DateTime.Now;
                _CidbContext.SaveChanges();

            }

        }

        public int RemoveStory(int StoryId)
        {
            var removeStory = _CidbContext.Stories.Where(u => u.StoryId == StoryId).FirstOrDefault();
            if (removeStory != null)
            {
                removeStory.DeletedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;

            }
            return 0;
        }

        public Story updateStoryByDTb(string storyid, string columnName, string columndata)
        {
            columndata = HttpUtility.UrlEncode(columndata);

            var story = _CidbContext.Stories.Where(m => m.StoryId.ToString() == storyid).FirstOrDefault();
            var property = typeof(Story).GetProperty(columnName);
            if (property != null)
            {

                var convertedValue = Convert.ChangeType(columndata, property.PropertyType);

                property.SetValue(story, convertedValue);
                _CidbContext.SaveChanges();
            }
            return story;
        }

        //Themes
        public IPagedList<ThemePageForAdminViewModel> getThemes(string searchkeyword, int pageIndex)
        {
            var Themedata = _CidbContext.Themes.Where(m => (searchkeyword == null || m.Title.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
            var ThemeDetails = new List<ThemePageForAdminViewModel>();
            foreach (var theme in Themedata)
            {
                var EachTheme = new ThemePageForAdminViewModel
                {
                    ThemeId = theme.ThemeId,
                    ThemeTitle = theme.Title,
                    Status = theme.Status,

                };
                ThemeDetails.Add(EachTheme);
            }


            return ThemeDetails.ToPagedList(pageIndex, 9);
        }
        public int RemoveTheme(int ThemeId)
        {
            /*            var mission_chk = _CidbContext.Missions.Where(t=>t.ThemeId == ThemeId).Count();
            */
            var mission_chk = _CidbContext.Themes.Include(m => m.Missions).Where(m => m.ThemeId == ThemeId).FirstOrDefault();
            if (mission_chk != null)
            {
                if (mission_chk.Missions.Where(m => m.ThemeId == ThemeId).Count() == 0)
                {
                    mission_chk.DeletedAt = DateTime.Now;
                    _CidbContext.SaveChanges();
                    return 1;
                }
            }
            return 0;


        }

        public ThemePageForAdminViewModel GetThemeData(int themeid)
        {
            var themedata = _CidbContext.Themes.Where(t => t.ThemeId == themeid).FirstOrDefault();
            var StoreTheme = new ThemePageForAdminViewModel();
            StoreTheme.ThemeId = themedata.ThemeId;
            StoreTheme.ThemeTitle = themedata.Title;
            StoreTheme.Status = themedata.Status;
            return StoreTheme;
        }

        public int AddTheme(ThemePageForAdminViewModel themePageForAdminView)
        {
            var themeTitlechk = _CidbContext.Themes.Where(t => t.DeletedAt == null).ToList();
            var flag = 1;
            foreach (var title in themeTitlechk)
            {
                if (themePageForAdminView.ThemeTitle.ToLower() == title.Title.ToLower())
                {
                    flag = 0;
                    break;
                }
            }
            if (flag == 1)
            {


                var theme = new Theme();
                theme.Title = themePageForAdminView.ThemeTitle;
                theme.Status = themePageForAdminView.Status;
                theme.CreatedAt = DateTime.Now;
                _CidbContext.Themes.Add(theme);
                _CidbContext.SaveChanges();
            }
            return flag;
        }
        public int EditTheme(ThemePageForAdminViewModel themePageForAdminView)
        {
            var themeTitlechk = _CidbContext.Themes.Include(m => m.Missions).Where(t => t.DeletedAt == null).ToList();
            var flag = 1;
            foreach (var title in themeTitlechk)
            {
                if (themePageForAdminView.ThemeTitle.ToLower() == title.Title.ToLower())
                {
                    flag = 0;
                    break;
                }
            }
            var themedata = themeTitlechk.Where(t => t.ThemeId == themePageForAdminView.ThemeId).FirstOrDefault();


            if (themedata.Missions.Where(t => t.ThemeId == themePageForAdminView.ThemeId).Count() != 0 && themePageForAdminView.Status == 2)
            {
                return 0;
            }
            if (flag == 1 || themedata.Title.ToLower() == themePageForAdminView.ThemeTitle.ToLower() && themedata.Status != themePageForAdminView.Status)
            {

                themedata.Title = themePageForAdminView.ThemeTitle;
                themedata.Status = themePageForAdminView.Status;
                themedata.UpdatedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;
            }
            else
            {
                return 2; //Title is present
            }
        }
        // Skillls
        public IPagedList<SkillPageForAdminViewModel> getSkills(string searchkeyword, int pageIndex)
        {
            var Skilldata = _CidbContext.Skills.Where(m => (searchkeyword == null || m.SkillName.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
            var ThemeDetails = new List<SkillPageForAdminViewModel>();
            foreach (var skill in Skilldata)
            {
                var EachTheme = new SkillPageForAdminViewModel
                {
                    Skillid = skill.SkillId,
                    SkillTitle = skill.SkillName,
                    Status = skill.Status,

                };
                ThemeDetails.Add(EachTheme);
            }


            return ThemeDetails.ToPagedList(pageIndex, 9);
        }
        public int RemoveSkill(int skillId)
        {
            var skill_chk = _CidbContext.Skills.Include(m => m.MissionSkills).Include(s => s.UserSkills).Where(m => m.SkillId == skillId).FirstOrDefault();
            if (skill_chk.MissionSkills.Where(m => m.SkillId == skillId).Count() == 0 && skill_chk.UserSkills.Where(m => m.SkillId == skillId).Count() == 0)
            {
                skill_chk.DeletedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        public SkillPageForAdminViewModel GetSkillData(int skillId)
        {
            var Skilldata = _CidbContext.Skills.Where(t => t.SkillId == skillId).FirstOrDefault();
            var Skills = new SkillPageForAdminViewModel();
            Skills.Skillid = Skilldata.SkillId;
            Skills.SkillTitle = Skilldata.SkillName;
            Skills.Status = Skilldata.Status;
            return Skills;
        }
        public int AddSkill(SkillPageForAdminViewModel skillPageForAdminViewModel)
        {
            var SkillTitlechk = _CidbContext.Skills.Where(t => t.DeletedAt == null).ToList();
            var flag = 1;
            foreach (var title in SkillTitlechk)
            {
                if (skillPageForAdminViewModel.SkillTitle.ToLower() == title.SkillName.ToLower())
                {
                    flag = 0;
                    break;
                }
            }
            if (flag == 1)
            {


                var skill = new Skill();
                skill.SkillName = skillPageForAdminViewModel.SkillTitle;

                skill.Status = skillPageForAdminViewModel.Status;
                skill.CreatedAt = DateTime.Now;
                _CidbContext.Add(skill);
                _CidbContext.SaveChanges();
            }
            return flag;
        }

        public int EditSkill(SkillPageForAdminViewModel skillPageForAdminViewModel)
        {
            var skillTitlechk = _CidbContext.Skills.Include(m => m.MissionSkills).Include(u => u.UserSkills).Where(t => t.DeletedAt == null).ToList();
            var flag = 1;
            foreach (var skill in skillTitlechk)
            {
                if (skillPageForAdminViewModel.SkillTitle.ToLower() == skill.SkillName.ToLower())
                {
                    flag = 0;
                    break;
                }
            }
            var skilldata = skillTitlechk.Where(t => t.SkillId == skillPageForAdminViewModel.Skillid).FirstOrDefault();


            if ((skilldata.MissionSkills.Where(t => t.SkillId == skillPageForAdminViewModel.Skillid).Count() != 0 || skilldata.UserSkills.Where(t => t.SkillId == skillPageForAdminViewModel.Skillid).Count() != 0) && skillPageForAdminViewModel.Status == 2)
            {
                return 0;
            }
            if (flag == 1 || skilldata.SkillName.ToLower() == skillPageForAdminViewModel.SkillTitle.ToLower() && skilldata.Status != skillPageForAdminViewModel.Status)
            {

                skilldata.SkillName = skillPageForAdminViewModel.SkillTitle;
                skilldata.Status = skillPageForAdminViewModel.Status;
                skilldata.UpdatedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;
            }
            else if (skillPageForAdminViewModel.SkillTitle == skilldata.SkillName && skillPageForAdminViewModel.Status == skilldata.Status)
            {
                return 3; //Title already is present
            }
            else
                return 2;
        }

        //Banner
        public IPagedList<BannerPageForAdminViewModel> getBanners(string searchkeyword, int pageIndex)
        {
            var BannerData = _CidbContext.Banners.Where(m => (searchkeyword == null || m.Title.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
            var BannerDetails = new List<BannerPageForAdminViewModel>();
            foreach (var banner in BannerData)
            {
                var EachBanner = new BannerPageForAdminViewModel
                {
                    BannerId = banner.BannerId,
                    Title = banner.Title,
                    Image = banner.Image,
                    Text = banner.Text,
                    SortOrder = banner.SortOrder,
                    Status = banner.Status,

                };
                BannerDetails.Add(EachBanner);
            }


            return BannerDetails.ToPagedList(pageIndex, 5);
        }
        public BannerPageForAdminViewModel GetBannerData(int bannerId)
        {

            var bannerdata = _CidbContext.Banners.Where(t => t.BannerId == bannerId).FirstOrDefault();
            var Banner = new BannerPageForAdminViewModel();
            Banner.Title = bannerdata.Title;
            Banner.Text = bannerdata.Text;
            Banner.SortOrder = bannerdata.SortOrder;
            Banner.Status = bannerdata.Status;
            Banner.BannerId = bannerdata.BannerId;
            Banner.Image = bannerdata.Image;
            return Banner;
        }


        public int RemoveBanner(int bannerId)
        {
            var removebanner = _CidbContext.Banners.Where(u => u.BannerId == bannerId).FirstOrDefault();
            if (removebanner != null)
            {
                removebanner.DeletedAt = DateTime.Now;
                _CidbContext.SaveChanges();
                return 1;

            }
            return 0;
        }
        public void AddBanner(BannerPageForAdminViewModel bannerPageForAdminView, IFormFileCollection IFormImg)
        {
            var BannerData = new Banner();
            BannerData.Title = bannerPageForAdminView.Title;
            BannerData.Text = bannerPageForAdminView.Text;
            BannerData.SortOrder = bannerPageForAdminView.SortOrder;
            if (IFormImg.Count > 0)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(IFormImg[0].FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedImages", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    BannerData.Image = "/UploadedImages/" + ImageName;
                    IFormImg[0].CopyTo(stream);
                }
            }
            _CidbContext.Add(BannerData);
            _CidbContext.SaveChanges();
        }
        public void EditBanner(BannerPageForAdminViewModel bannerPageForAdminView, IFormFileCollection IFormImg)
        {

            var bannerData = _CidbContext.Banners.Where(t => t.BannerId == bannerPageForAdminView.BannerId).FirstOrDefault();


            bannerData.Title = bannerPageForAdminView.Title;
            bannerData.Status = bannerPageForAdminView.Status;
            bannerData.SortOrder = bannerPageForAdminView.SortOrder;
            bannerData.Text = bannerPageForAdminView.Text;
            if (IFormImg.Count > 0)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(IFormImg[0].FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedImages", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    bannerData.Image = "/UploadedImages/" + ImageName;
                    IFormImg[0].CopyTo(stream);
                }
            }
            bannerData.UpdatedAt = DateTime.Now;
            _CidbContext.SaveChanges();
        }

        //CMS Page
        public IPagedList<CMSPageForAdminViewModel> getCMSList(string searchkeyword, int pageIndex)
        {
            /*            var cmsData = _CidbContext.CmsPages.Where(m => (searchkeyword == null || m.Title.Contains(searchkeyword)) && m.DeletedAt == null).ToList();
             *            
            */
            if (searchkeyword == null)
                searchkeyword = "";

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@searchkeyword",searchkeyword),
            };
            var cmsData = _CidbContext.CmsPages.FromSqlRaw("EXEC GetCMSdata @searchkeyword", parameter).ToList();
            var CMSDetails = new List<CMSPageForAdminViewModel>();
            foreach (var cms in cmsData)
            {
                var EachCMS = new CMSPageForAdminViewModel
                {
                    CmsPageId = cms.CmsPageId,
                    Title = cms.Title,
                    Status = cms.Status,

                };
                CMSDetails.Add(EachCMS);
            }


            return CMSDetails.ToPagedList(pageIndex, 9);
        }
        public CMSPageForAdminViewModel GetCMSData(int cmsId)
        {
            /*  var cmsdata = _CidbContext.CmsPages.Where(t => t.CmsPageId == cmsId).FirstOrDefault(); */

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@cmsId",cmsId),
            };

            var cmsdata = _CidbContext.CmsPages.FromSqlRaw("EXEC GetCMSById @cmsId", parameter).ToList().FirstOrDefault();
            var CMS = new CMSPageForAdminViewModel();
            CMS.CmsPageId = cmsId;
            CMS.Title = cmsdata.Title;
            CMS.Status = cmsdata.Status;
            CMS.Description = cmsdata.Description;
            CMS.Slug = cmsdata.Slug;

            return CMS;
        }

        public void EditCMS(CMSPageForAdminViewModel cmsPageForAdminViewModel)
        {
            /*            var cmsData = _CidbContext.CmsPages.Where(t => t.CmsPageId == cmsPageForAdminViewModel.CmsPageId).FirstOrDefault();
            */
            SqlParameter[] parameter = new SqlParameter[]
         {
                new SqlParameter("@cmsId",cmsPageForAdminViewModel.CmsPageId),
                new SqlParameter("@description",cmsPageForAdminViewModel.Description),
                new SqlParameter("@title",cmsPageForAdminViewModel.Title),
                new SqlParameter("@slug",cmsPageForAdminViewModel.Slug),
                new SqlParameter("@status",cmsPageForAdminViewModel.Status),
         };

            var cmsData = _CidbContext.Database.ExecuteSqlRaw("EXEC UpdateCMS @cmsId, @description, @title, @slug, @status", parameter);
            /* cmsData.Title = cmsPageForAdminViewModel.Title;
             cmsData.Status = cmsPageForAdminViewModel.Status;
             cmsData.Slug = cmsPageForAdminViewModel.Slug;
             cmsData.Description = cmsPageForAdminViewModel.Description;

             cmsData.UpdatedAt = DateTime.Now;
             _CidbContext.SaveChanges();*/
        }
        public void AddCMS(CMSPageForAdminViewModel cmsPageForAdminViewModel)
        {
            SqlParameter[] parameter = new SqlParameter[]
         {
                new SqlParameter("@description",cmsPageForAdminViewModel.Description),
                new SqlParameter("@title",cmsPageForAdminViewModel.Title),
                new SqlParameter("@slug",cmsPageForAdminViewModel.Slug),
                new SqlParameter("@status",cmsPageForAdminViewModel.Status),
         };
            var cmsData = _CidbContext.Database.ExecuteSqlRaw("EXEC AddCMS @description, @title, @slug, @status", parameter);

            /* var CMSData = new CmsPage();
             CMSData.Slug = cmsPageForAdminViewModel.Slug;
             CMSData.Status = cmsPageForAdminViewModel.Status;
             CMSData.Description = cmsPageForAdminViewModel.Description;
             CMSData.Title = cmsPageForAdminViewModel.Title;


             _CidbContext.Add(CMSData);
             _CidbContext.SaveChanges();*/
        }
        public int RemoveCMS(int cmsId)
        {
            /* var removecms = _CidbContext.CmsPages.Where(u => u.CmsPageId == cmsId).FirstOrDefault();
             if (removecms != null)
             {
                 removecms.DeletedAt = DateTime.Now;
                 _CidbContext.SaveChanges();
                 return 1;

             }*/
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@cmsId",cmsId),
            };

            var cmsdata = _CidbContext.Database.ExecuteSqlRaw("EXEC CMS_RemoveCmS @cmsId", parameter);
            return 0;
        }
        public int removeCheckedcms(List<int> checkedCmsValues)
        {
            var cmsRecords = _CidbContext.CmsPages.Where(cms => checkedCmsValues.Contains((int)cms.CmsPageId));
            foreach (var id in cmsRecords)
            {
                id.DeletedAt = DateTime.Now;

            }
            _CidbContext.SaveChanges();
            return 0;
        }

        //Export to SQl
        public MappingColumns getColumnsMap(IFormFile file)
        {
            MappingColumns mapping = new MappingColumns();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


                    List<Admin> data = new List<Admin>();
                    DataTable dataTable = new DataTable();

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string column1Value = worksheet.Cells[row, 1].Value.ToString();
                        string column2Value = worksheet.Cells[row, 2].Value.ToString();
                        string column3Value = worksheet.Cells[row, 3].Value.ToString();
                        string column4Value = worksheet.Cells[row, 4].Value.ToString();

                        data.Add(new Admin
                        {
                            FirstName = column1Value,
                            LastName = column2Value,
                            Email = column3Value,
                            Password = column4Value,

                        });
                    }
                    // Convert your List<Admin> data to a DataTable
                    dataTable.Columns.Add(worksheet.Cells[1, 1].Value.ToString(), typeof(string));
                    dataTable.Columns.Add(worksheet.Cells[1, 2].Value.ToString(), typeof(string));
                    dataTable.Columns.Add(worksheet.Cells[1, 3].Value.ToString(), typeof(string));
                    dataTable.Columns.Add(worksheet.Cells[1, 4].Value.ToString(), typeof(string));

                    foreach (var admin in data)
                    {
                        dataTable.Rows.Add(admin.FirstName, admin.LastName, admin.Email, admin.Password);
                    }

                    mapping.tableValues = dataTable;

                }
            }
            return mapping;
        }
        public int exportToSql(DataTable dataTable, List<string> selectedIndicesList)
        {
            using (SqlConnection connection = (SqlConnection)_CidbContext.Database.GetDbConnection())
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    //Using a array having index of column name's for mapping
                    bulkCopy.DestinationTableName = "Admin";
                    var schema = connection.GetSchema("Columns", new[] { null, null, "Admin" });
                    var columnNames = schema.AsEnumerable().Select(row => row["COLUMN_NAME"].ToString()).ToList();
                    bulkCopy.ColumnMappings.Add(selectedIndicesList[0], columnNames[4]);
                    bulkCopy.ColumnMappings.Add(selectedIndicesList[1], columnNames[5]);
                    bulkCopy.ColumnMappings.Add(selectedIndicesList[2], columnNames[0]);
                    bulkCopy.ColumnMappings.Add(selectedIndicesList[3], columnNames[1]);

                    /*     Direct with DataTable
                     *     bulkCopy.DestinationTableName = "Admin";
                          bulkCopy.ColumnMappings.Add(dataTable.Columns[0].ColumnName, "First_name");
                          bulkCopy.ColumnMappings.Add(dataTable.Columns[1].ColumnName, "Last_name");
                          bulkCopy.ColumnMappings.Add(dataTable.Columns[2].ColumnName, "Email");
                          bulkCopy.ColumnMappings.Add(dataTable.Columns[3].ColumnName, "Password");*/
                    bulkCopy.WriteToServer(dataTable);

                }
            }
            return 1;

        }


    }


}
