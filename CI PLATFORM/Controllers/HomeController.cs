using CI_PLATFORM.Models;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
/*            var CMSdata = _iuserInterface.cmsData();
*/            return View();
        }
       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult SearchTablePage()
        {
            List<UsersViewModel> users = SearchUser("");
            return View(users);
        }
        [HttpPost]
        public IActionResult SearchTablePage(string name)
        {
            List<UsersViewModel> users = SearchUser(name);
            return View(users);
        }
        private static List<UsersViewModel> SearchUser(string name)
        {
            List<UsersViewModel> users = new();
            //Method 1
            /*  string apiUrl = $"https://localhost:44381/api/CustomAPI/GetUsers?name={Uri.EscapeDataString(name)}";
              HttpClient client = new();
              HttpResponseMessage responseMessage = client.GetAsync(apiUrl).Result;*/

            //Method 2

            string apiUrl = "https://localhost:44381/api/CustomAPI"; 
            HttpClient client = new HttpClient();
            HttpResponseMessage message = new();
           if (name== "" || name==null)
            {
                 message = client.GetAsync(apiUrl + string.Format("/GetAlllUsers")).Result;

            }
           else
            {
                message = client.GetAsync(apiUrl + string.Format("/GetUsers?name={0}", name)).Result;

            }

            if (message.IsSuccessStatusCode)
            {
                users = JsonConvert.DeserializeObject<List<UsersViewModel>>(message.Content.ReadAsStringAsync().Result);
            }
           
            return users;

        }


        public IActionResult MapApi()
        {
            return View();
        }
        public IActionResult SelectForm()
        {
            return View();
        }
        public IActionResult GoogleCharts()
        {
            return View();
        }
    }
}