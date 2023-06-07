using ciplatform.entities.Data;
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI_PLATFORM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAPIController : ControllerBase
    {
        private CidbContext CidbContext { get; }
        private readonly IAdminInterface iAdminInterface;

        public CustomAPIController ( CidbContext _cidbContext, IAdminInterface _iAdminInterface)
        {
            this.CidbContext = _cidbContext;
            this.iAdminInterface = _iAdminInterface;
        }

        [Route("GetUsers")]
        [HttpGet]
        public List<User> GetUsers(string name)
        {
           
            return CidbContext.Users.Where(m=>m.FirstName.Contains(name) || m.SecondName.Contains(name)).ToList();
        }
        [Route("GetAlllUsers")]
        [HttpGet]
        public List<User> GetUsers()
        {
           
            return CidbContext.Users.ToList();
        }
        [Route ("AddEditCms")]
        [HttpPost]
        public void AddEditCms([FromBody]CMSPageForAdminViewModel cmsPageForAdminViewModel)
        {

            if (cmsPageForAdminViewModel.CmsPageId != 0)
            {
                iAdminInterface.EditCMS(cmsPageForAdminViewModel);

            }
            else
            {
                iAdminInterface.AddCMS(cmsPageForAdminViewModel);

            }
        }
       
    }
}
