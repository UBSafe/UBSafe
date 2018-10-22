using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBSafeAPI.Models
{
    public class Location
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
        public DateTime LastUpdated { get; set; }

        public Location(float lat, float lon, DateTime lastUpdated)
        {
            this.Lat = lat;
            this.Lon = lon;
            this.LastUpdated = lastUpdated;
        }
    }
}
