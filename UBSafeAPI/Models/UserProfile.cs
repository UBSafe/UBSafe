
namespace UBSafeAPI.Models
{
    public class UserProfile
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public UserProfile(User user)
        {
            UserID = user.UserID;
            UserName = user.UserName;
            Age = user.Age;
            Gender = user.Gender;
        }

    }
}
