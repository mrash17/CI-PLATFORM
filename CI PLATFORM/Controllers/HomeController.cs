using CI_PLATFORM.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_PLATFORM.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserInterface _iuserInterface;


        public HomeController(ILogger<HomeController> logger, IUserInterface iuserInterface)

        {
            _iuserInterface = iuserInterface;

            _logger = logger;
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
              
        public IActionResult lostpas()
        {
            return View();
        }
        public IActionResult resetpass()
        {
            return View();
        }

        public IActionResult register()
        {
            return View();
        }
        public IActionResult PageNotFound()
        {
            return View();
        }

     
        public IActionResult nomissionfound()
        {
            return View();
        }
        public IActionResult PrivacyPolicy()
        {
            var CMSdata = _iuserInterface.cmsData();
            return View(CMSdata);
        }
       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}