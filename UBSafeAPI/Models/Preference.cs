using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class Preference
    {
        public Preference(int ageMin, int ageMax, float proximity, bool femaleCompanionsOkay, bool maleCompanionsOkay, bool otherCompanionsOkay)
        {
            AgeMin = ageMin;
            AgeMax = ageMax;
            Proximity = proximity;
            FemaleCompanionsOkay = femaleCompanionsOkay;
            MaleCompanionsOkay = maleCompanionsOkay;
            OtherCompanionsOkay = otherCompanionsOkay;
        }

        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public double Proximity { get; set; }
        public bool FemaleCompanionsOkay { get; set; }
        public bool MaleCompanionsOkay { get; set; }
        public bool OtherCompanionsOkay { get; set; }
    }
}
