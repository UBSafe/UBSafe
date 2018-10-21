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
        public Gender Gender { get; set; }
        public float Proximity { get; set; }
    }
}
