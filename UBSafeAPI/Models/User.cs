using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class User
    {
        //public int UserID { get; set; }
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

        public User(string userName, int age, string gender, float lat, float lon, int prefAgeMin, int prefAgeMax, float prefProximity, bool femaleCompanionsOkay, bool maleCompanionsOkay, bool otherCompanionsOkay)
        {
            this.UserName = userName;
            this.Age = age;
            this.Gender = gender;
            this.Location = new Location(lat, lon, DateTime.Now);
            this.PrefAgeMin = prefAgeMin;
            this.PrefAgeMax = prefAgeMax;
            this.PrefProximity = prefProximity;
            this.FemaleCompanionsOkay = femaleCompanionsOkay;
            this.MaleCompanionsOkay = maleCompanionsOkay;
            this.OtherCompanionsOkay = otherCompanionsOkay;
        }
    }
}
