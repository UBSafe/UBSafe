using System;

namespace UBSafeAPI.Models.SideGeoFire
{
    public class GeoFireException : Exception
    {
        public GeoFireException(string message)
            : base(message)
        {
        }

        public GeoFireException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}