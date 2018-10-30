
namespace UBSafeAPI.Models
{
    public class UserProfile
    {
        private string UserID { get; set; }
        private string UserName { get; set; }
        private int Age { get; set; }
        private string Gender { get; set; }

        public UserProfile(User user)
        {
            UserID = user.UserID;
            UserName = user.UserName;
            Age = user.Age;
            Gender = user.Gender;
        }

    }
}
