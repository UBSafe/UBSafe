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
        public Preference Preferences { get; set; }
    }
}
