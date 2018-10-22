using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class Preference
    {
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public float Proximity { get; set; }
        public GenderPreferences GenderPreferences { get; set; }

        public Preference(int ageMin, int ageMax, float proximity, GenderPreferences genderPreferences)
        {
            this.AgeMin = ageMin;
            this.AgeMax = ageMax;
            this.Proximity = proximity;
            this.GenderPreferences = genderPreferences;
        }
    }

    public class GenderPreferences
    {
        public bool FemaleCompanions { get; set; }
        public bool MaleCompanions { get; set; }
        public bool OtherCompanions { get; set; }

        public GenderPreferences(bool female, bool male, bool other)
        {
            this.FemaleCompanions = female;
            this.MaleCompanions = male;
            this.OtherCompanions = other;
        }
    }
}
