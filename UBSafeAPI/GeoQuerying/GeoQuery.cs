using System;
using SideGeoFire;

namespace UBSafeAPI.GeoQuerying
{

    public class GeoQuery
    {

        public string LowerGeoHash;
        public string UpperGeoHash;

        private double CenterLat;
        private double CenterLon;
        private double Radius;

        private double SWCornerLat;
        private double SWCornerLon;
        private double NECornerLat;
        private double NECornerLon;

        public GeoQuery(double centerLat, double centerLon, double proximity)
        {
            this.CenterLat = centerLat;
            this.CenterLon = centerLon;
            this.Radius = proximity;

            SetBoundingBoxCoordinates();
            this.LowerGeoHash = GeoFire.BuildGeoHash(SWCornerLat, SWCornerLon);
            this.UpperGeoHash = GeoFire.BuildGeoHash(NECornerLat, NECornerLon);
        }

        private void SetBoundingBoxCoordinates()
        {
            const double KM_PER_DEGREE_LATITUDE = 110.574;
            double latDegrees = this.Radius / KM_PER_DEGREE_LATITUDE;
            double latitudeNorth = Math.Min(90, this.CenterLat + latDegrees);
            double latitudeSouth = Math.Max(-90, this.CenterLat - latDegrees);
            // calculate longitude based on current latitude
            double longDegsNorth = MetersToLongitudeDegrees(this.Radius, latitudeNorth);
            double longDegsSouth = MetersToLongitudeDegrees(this.Radius, latitudeSouth);
            double longDegs = Math.Max(longDegsNorth, longDegsSouth);

            this.SWCornerLat = latitudeSouth;
            this.SWCornerLon = WrapLongitude(this.CenterLon - longDegs);
            this.NECornerLat = latitudeNorth;
            this.NECornerLon = WrapLongitude(this.CenterLon + longDegs); 
        }

        private double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private double MetersToLongitudeDegrees(double distance, double latitude)
        {
            const double EARTH_EQ_RADIUS = 6378137.0;
            const double E2 = 0.00669447819799; //magic number taken from the GeoFire library
            const double EPSILON = 1*10^-12;

            double radians = DegreesToRadians(latitude);
            double num = Math.Cos(radians) * EARTH_EQ_RADIUS * Math.PI / 180;
            double denom = 1 / Math.Sqrt(1 - E2 * Math.Sin(radians) * Math.Sin(radians));
            double deltaDeg = num * denom;

            if(deltaDeg < EPSILON)
            {
                return (distance > 0) ? 360 : 0;
            }
            else
            {
                return Math.Min(360, (distance / deltaDeg));
            }
        }

        /**
        * Wraps the longitude to [-180,180].
        *
        * @param {number} longitude The longitude to wrap.
        * @return {number} longitude The resulting longitude.
        */
        public double WrapLongitude(double longitude)
        {
            if (longitude <= 180 && longitude >= -180)
            {
                return longitude;
            }

            double adjusted = longitude + 180;
            if (adjusted > 0)
            {
                return (adjusted % 360) - 180;
            }
            else
            {
                return 180 - (-adjusted % 360);
            }
        }


    }


}
