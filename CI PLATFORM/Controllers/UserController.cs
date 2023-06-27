using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CI_PLATFORM.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserInterface _iuserInterface;
        private readonly CidbContext _Cidbcontext;
        public UserController(IUserInterface iuserInterface, CidbContext _dbContext)
        {
            _iuserInterface = iuserInterface;
            _Cidbcontext = _dbContext;
        }

        public string? FirstName { get; private set; }

        public IActionResult Index()
        {
            var bannerList = _iuserInterface.GetBanner();
            return View(bannerList);
        }
    /*    public JsonResult GetBanner()
        {
            var bannerList = _iuserInterface.GetBanner();
            return Json(bannerList);

        }*/
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(User userdetails)
        {


            int chk = _iuserInterface.Index(userdetails);
            var bannerList = _iuserInterface.GetBanner();
            if (chk == -1)
            {
                TempData["Nregister"] = "You are not a Registered User";
                return View(bannerList);

            }
            else if (chk == 0)
            {
                var credvalid = _Cidbcontext.Users.Where(model => model.Email == userdetails.Email && model.Password == userdetails.Password).FirstOrDefault();

                if (credvalid == null)
                {
                    TempData["Nregister"] = "Enter valid Password";
                }
                return View(bannerList);  
            }
            else
            {
             
                var credvalid = _Cidbcontext.Users.FirstOrDefault(model => model.Email == userdetails.Email && model.Password == userdetails.Password && model.DeletedAt==null && model.Status==true);
                if (credvalid != null)
                {
                    var uservalue = _Cidbcontext.Users.FirstOrDefault(model => model.Email == userdetails.Email);

                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, userdetails.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, uservalue.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, uservalue.SecondName));
                    if(uservalue.Avatar==null)
                    {
                        uservalue.Avatar = "\\images\\user1.png";
                    }
                    identity.AddClaim(new Claim(ClaimTypes.UserData, uservalue.Avatar));
                    var principle = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
                    string SessionUserid = credvalid.UserId.ToString();
                    HttpContext.Session.SetString("userId", credvalid.Email);
                    HttpContext.Session.SetString("sessionuserid", SessionUserid);

                    TempData["login"] = "Login Sucessful";
                    return RedirectToAction("landingpage", "LandingPage");
                }
                //Admin
                else
                {
                    var adminvalid = _iuserInterface.AdminChk(userdetails);
                    if(adminvalid.Count !=0)
                    {
                        var admin=adminvalid.FirstOrDefault();
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, userdetails.Email) },
                        CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, admin.FirstName));
                        identity.AddClaim(new Claim(ClaimTypes.Surname, admin.LastName));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        identity.AddClaim(new Claim(ClaimTypes.UserData, "\\images\\user-img.png"));

                        var adminprinciple = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminprinciple);
                        string SessionAdminId = admin.AdminId.ToString();
                        HttpContext.Session.SetString("AdminEmail", admin.Email);
                        HttpContext.Session.SetString("sessionAdminid", SessionAdminId);

                        TempData["login"] = "Login Sucessful";
                        return RedirectToAction("UserPage", "Admin");
                    }
                    else
                    {
                        TempData["Nregister"] = "You are not a valid user";

                        return View(bannerList);
                    }
                }
               
            }

        }

        [HttpGet]
        public IActionResult register()
        {
            var bannerList = _iuserInterface.RGetBanner();
            return View(bannerList);
        }
        [HttpPost]
            public IActionResult Register(RegisterationViewModel user)
            {
            var bannerList = _iuserInterface.RGetBanner();
            user.bannerList = bannerList.bannerList;
            int UserExists = _iuserInterface.Register(user);

                if (UserExists == 0)
                {
                    TempData["Error"] = "You are already a Registered User";

                    return View(user);
                }
                if (UserExists == 1)
                {
                    TempData["registered"] = "Registered Sucessfully";
                    return RedirectToAction("Index", "User");
                }
                return View(user);
           

           }
       
        public IActionResult lostpas()
        {
            var bannerList = _iuserInterface.LGetBanner();
            

            return View(bannerList);
        }


        public IActionResult resetpass()
        {
            var bannerList = _iuserInterface.GetBanner();

            return View(bannerList);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("landingpage", "LandingPage");
        }

        public IActionResult Userdetails()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");

            if (userid == null)
            {
               return RedirectToAction("Index", "User");
            }
            else
            {
                var skills = _iuserInterface.getSkill(userid);
                return View(skills);
            }
        }
        public JsonResult GetUserData()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");

/*            userid = "44";
*/            var userdetails = _iuserInterface.Getuserdata(userid);
            return Json(userdetails);
            
        }
        public IActionResult SaveUserDetails(UserDetailsViewModel userDetailsViewModel)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            _iuserInterface.SaveUserData(userDetailsViewModel,userid);
            return RedirectToAction("Userdetails");
        }
        public JsonResult GetAllCountries(int cid)
        {
            var countries = _iuserInterface.GetCountryList(cid);
            return Json(countries);
        }  public JsonResult GetAllCity(int id,int cityid)
        {
            var cities = _iuserInterface.GetCityList(id,cityid);
            return Json(cities);
        }
        public IActionResult Changepassword(UserDetailsViewModel userDetailsViewModel)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");

            var result = _iuserInterface.UpdatePassword(userDetailsViewModel,userid);
            if(result == "Updated")
            {
                TempData["success"] = "Password Changed";
            }
            else
            {
                TempData["Error"] = "Incorrect Password";
            }
            return RedirectToAction("Userdetails");
        }
        public IActionResult ContactusSubmit(ContactUsViewModel contactUsViewModel)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
             _iuserInterface.ContactUs(userid, contactUsViewModel);
            return RedirectToAction("Userdetails");
        }
        public IActionResult AddImage(IFormFile image)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            if(userid == null)
            {
                return RedirectToAction("Index","User");

            }

            _iuserInterface.editImage(image,int.Parse(userid));
            return RedirectToAction("Userdetails");
        }


        public JsonResult GetTitles()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            var titles = _iuserInterface.gettitles(userid);
            return Json(new { titles = titles.Item1, ids = titles.Item2 });
        }


        public void SetStatus(List<string> titles)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            _iuserInterface.setstatus(userid, titles);

        }
        public void ChangeStatus(int messageid)
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            _iuserInterface.changestatus(messageid, userid);
        }
        public JsonResult GetNotification()
        {
            var userid = HttpContext.Session.GetString("sessionuserid");
            var notifications = _iuserInterface.getnotification(userid);
            return Json(notifications);
        }

        public JsonResult GetAllApplications()
        {
            var applications = _Cidbcontext.MissionApplications.Where(m => m.ApprovalStatus == "approved" ).GroupBy(m=>m.MissionId);
            return Json(applications);
        }
    }

}
