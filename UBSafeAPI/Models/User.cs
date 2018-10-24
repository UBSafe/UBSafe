using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class User
    {
        public User(string userID, string userName, int age, string gender, int prefAgeMin, int prefAgeMax, float prefProximity, bool femaleCompanionsOkay, bool maleCompanionsOkay, bool otherCompanionsOkay, Location location)
        {
            UserID = userID;
            UserName = userName;
            Age = age;
            Gender = gender;
            PrefAgeMin = prefAgeMin;
            PrefAgeMax = prefAgeMax;
            PrefProximity = prefProximity;
            FemaleCompanionsOkay = femaleCompanionsOkay;
            MaleCompanionsOkay = maleCompanionsOkay;
            OtherCompanionsOkay = otherCompanionsOkay;
            Location = location;
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Location Location { get; set; }
        public int PrefAgeMin { get; set; }
        public int PrefAgeMax { get; set; }
        public float PrefProximity { get; set; }
        public bool FemaleCompanionsOkay { get; set; }
        public bool MaleCompanionsOkay { get; set; }
        public bool OtherCompanionsOkay { get; set; }



        public UserProfile getProfile()
        {
            return new UserProfile(this.UserID, this.UserName, this.Age, this.Gender);
        }
    }

    public class UserProfile
    {
        public UserProfile(string userID, string userName, int age, string gender)
        {
            UserID = userID;
            UserName = userName;
            Age = age;
            Gender = gender;
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
