using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModel;
using ciplatform.repository.Interface;

using ciplatform.entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CI_PLATFORM.Controllers
{
    public class LostpassController : Controller
    {
        private readonly CidbContext _CidbContext;
        private readonly IUserInterface _iuserInterface;

        public LostpassController(CidbContext CidbContext, IUserInterface iuserInterface)
        {
            _CidbContext = CidbContext;
            _iuserInterface = iuserInterface;
        }
        public IActionResult lostpas()
        {
            var bannerList = _iuserInterface.LGetBanner();

            return View(bannerList);
        }


        [HttpPost]
        public IActionResult lostpas(LostpasViewModel user)
        {
                var validEmail = _CidbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            user.bannerList = _iuserInterface.LGetBanner().bannerList;
            
            if (validEmail != null)
                {
                    var token = Guid.NewGuid().ToString();

                    var passwordReset = new PasswordReset
                    {
                        Email = user.Email,
                        Token = token,
                    };

                    _CidbContext.Add(passwordReset);
                    _CidbContext.SaveChanges();

                    // Send an email with the password reset link to the user's email address
                    var resetLink = Url.Action("resetpass", "Lostpass", new { email = user.Email, token }, Request.Scheme);

                    // Send email to user with reset password link
                    // ...
                    var fromAddress = new MailAddress("makhijashish2000@gmail.com", "Ashish");
                    var toAddress = new MailAddress(user.Email);
                    var subject = "Password Reset request";
                    var body = $"Hi,<br /><br />Please click on the received link to reset your password:<br /><br /><a href='{resetLink}'>Reset Password</a>";

                    var message = new MailMessage(fromAddress, toAddress)
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
                    TempData["forget"] = "Reset link sent to Registered Email";
                    HttpContext.Session.SetString("token", token);

                    return RedirectToAction("Index", "User");
                }

                TempData["Error"] = "Please enter valid Email";
                return View(user);
            
                
        }
        //GetMethod
        [HttpGet]
        public IActionResult resetpass(string Email, string Token)
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return NotFound("link expired");

            }
            var banneList = _iuserInterface.LGetBanner().bannerList;

            var validEmail = _CidbContext.PasswordResets.FirstOrDefault(u => u.Email == Email && u.Token == Token);
            var newpasswordviewmodel = new Newpasswordviewmodel();

            if (validEmail != null)
            {


                newpasswordviewmodel.Token = token;
                newpasswordviewmodel.Email = Email;
                
            }
            newpasswordviewmodel.bannerList = banneList;

            return View(newpasswordviewmodel);
        }
        [HttpPost]
        public IActionResult resetpass(Newpasswordviewmodel newpasswordviewmodel)
        {
            var banneList = _iuserInterface.LGetBanner().bannerList;

            if (ModelState.IsValid)
            {
                var usercheck = _CidbContext.Users.FirstOrDefault(u => u.Email == newpasswordviewmodel.Email);
                if(usercheck != null)
                {
                    HttpContext.Session.Remove("token");
                    usercheck.Password = newpasswordviewmodel.NewPassword;
                    _CidbContext.SaveChanges();
                    TempData["reset"] = "Password Changed Successfully";
                    return RedirectToAction("Index", "User");

                }
                return View(banneList);

            }
            return View(banneList);
        }



    }
}
