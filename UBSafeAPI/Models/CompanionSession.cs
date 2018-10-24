using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class CompanionSession
    {
        public int sessionID { get; set; }
        public User Traveller { get; set; }
        public List<User> Watchers { get; set; }
        public Location Destination { get; set; }
        public DateTime LastUpdated { get; set; }
    }
    public class UpdateTrigger
    {
        public bool LocationUpdateTrigger { get; set; } 
    }
}
