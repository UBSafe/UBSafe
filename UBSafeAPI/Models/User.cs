
namespace UBSafeAPI.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Location Location { get; set; }
        public Preference Preferences { get; set; }


        public UserProfile GetProfile()
        {
            return new UserProfile(this);
        }
    }

}
