
using ciplatform.entities.Models;
using ciplatform.entities.ViewModels;

namespace ciplatform.repository.Interface
{
    public interface IVolunteerMissionInterface
    {
        public VolunteeringMissionViewModel GetVolunteerMission(int mid,string userid);
        public bool chkfavourite(int userid, int missionid);
        public string updateandrate(int missionid, int rating, int userid);
       public List<User> GetUsersbyid(int Userid);
        public string GetUsersId(int id, string url, int from_id, int mission_id);
        public string UserCommentPost(string commentstext, int MissionId, int userId);
        public string applymission(string userid, int MissionId);


    }
}
