using ciplatform.entities.Models;
using ciplatform.entities.ViewModel;
using ciplatform.entities.ViewModels;
using ciplatform.repository.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.repository.Interface
{
    public interface IUserInterface
    {
        public int Index(User userdetails);
        public UsersViewModel GetBanner();
        public RegisterationViewModel RGetBanner();
        public LostpasViewModel LGetBanner();
        // Register is file name // User is table name. registration is obj name
        public int Register(RegisterationViewModel registration);

        public int lostpas(User reqpas);

        public UserDetailsViewModel Getuserdata(string userid);
        public void SaveUserData(UserDetailsViewModel userDetailsViewModel, string userid);
        public List<Country> GetCountryList(int cid);
        public List<City> GetCityList(int id,int cityid);
        public UserDetailsViewModel getSkill(string userid);
        public string UpdatePassword(UserDetailsViewModel userDetailsViewModel, string userid);
        public void ContactUs( string userid, ContactUsViewModel contactUsViewModel);
        public void editImage(IFormFile image, int userid);
        public List<Admin> AdminChk(User userdetails);
        public CMSPageForAdminViewModel cmsData();



        public void changestatus(int messageid, string userid);
        public void setstatus(string userid, List<string> titles);
        public List<Tuple<string, long, string, string, int, int, string>> getnotification(string userId);
        public Tuple<List<NotificationTitle>, List<long>> gettitles(string userId);

    }



}
