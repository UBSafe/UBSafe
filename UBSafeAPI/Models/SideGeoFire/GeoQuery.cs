using System;

namespace UBSafeAPI.Models.SideGeoFire 
{

    public class GeoQuery
    {

        public readonly string lowerGeoHash;
        public readonly string upperGeoHash;

        private readonly double centerLat;
        private readonly double centerLon;
        private readonly double radius;

        private double swCornerLat;
        private double swCornerLon;
        private double neCornerLat;
        private double neCornerLon;

        public GeoQuery(double centerLat, double centerLon, double proximity)
        {
            this.centerLat = centerLat;
            this.centerLon = centerLon;
            this.radius = proximity;

            SetBoundingBoxCoordinates();
            this.lowerGeoHash = GeoFire.BuildGeoHash(swCornerLat, swCornerLon);
            this.upperGeoHash = GeoFire.BuildGeoHash(neCornerLat, neCornerLon);
        }

        private void SetBoundingBoxCoordinates()
        {
            const double KM_PER_DEGREE_LATITUDE = 110.574;
            double latDegrees = this.radius / KM_PER_DEGREE_LATITUDE;
            double latitudeNorth = Math.Min(90, this.centerLat + latDegrees);
            double latitudeSouth = Math.Max(-90, this.centerLat - latDegrees);
            // calculate longitude based on current latitude
            double longDegsNorth = MetersToLongitudeDegrees(this.radius, latitudeNorth);
            double longDegsSouth = MetersToLongitudeDegrees(this.radius, latitudeSouth);
            double longDegs = Math.Max(longDegsNorth, longDegsSouth);

            this.swCornerLat = latitudeSouth;
            this.swCornerLon = WrapLongitude(this.centerLon - longDegs);
            this.neCornerLat = latitudeNorth;
            this.neCornerLon = WrapLongitude(this.centerLon + longDegs); 
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private static double MetersToLongitudeDegrees(double distance, double latitude)
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
        public static double WrapLongitude(double longitude)
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
