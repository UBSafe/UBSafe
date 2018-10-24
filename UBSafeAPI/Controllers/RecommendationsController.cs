using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Firebase.Database;
using Firebase.Database.Query;
using GeoCoordinatePortable;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        static string auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita"; // app secret
        FirebaseClient firebase = new FirebaseClient(
          "https://ubsafe-a816e.firebaseio.com/",
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(auth)
          });

        // GET: api/Recommendations
        [HttpGet("{userID}")]
        public ActionResult Get([FromRoute] string userID)
        {
            //Get current user's preferences for querying purposes
            User traveller = firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;

            /*
             * If the user has a proximity preference, trigger location update in db - 
             * we do this before issuing other queries to give the 
             * clients time to update their locations in the db
             */
            if (traveller.PrefProximity != -1)
            {
                firebase.Child("LocationUpdateTrigger").PutAsync(true);
            }


            var returnedRecommendations = firebase
                                    .Child("Users")
                                    .OrderBy("Age")
                                    .StartAt(traveller.PrefAgeMin)
                                    .EndAt(traveller.PrefAgeMax)
                                    .OnceSingleAsync<Dictionary<string, User>>()
                                    .Result;

            // remove current user from list so that they don't get themselves as a recommendation
            returnedRecommendations.Remove(userID);

            //we received a json object with userID's as keys and users as values - we only 
            //need users now, so remove the keys
            IEnumerable<User> recommendations = returnedRecommendations.Values.ToList();

            if (!recommendations.Any())
            {
                return NotFound();
            }

            // remove current user from list so that they don't get themselves as a recommendation
            recommendations = recommendations.Where(user => !user.Equals(traveller));

            //filter by gender
            if (!traveller.MaleCompanionsOkay) recommendations = recommendations.Where(user => user.Gender != "Male");
            if (!traveller.FemaleCompanionsOkay) recommendations.Where(user => user.Gender != "Female");
            if (!traveller.OtherCompanionsOkay) recommendations.Where(user => user.Gender != "Other");

            //filter by location
            if(traveller.PrefProximity != -1 && recommendations.Any())
            {
                /*
                 * Location could be outdated, so discard users with locations
                 * that have not been updated in the last 24 hours
                 */
                DateTime now = DateTime.Now;
                recommendations = recommendations.Where(user => user.Location.LastUpdated > now.AddHours(-48) && user.Location.LastUpdated <= now);

                var travellerLoc = new GeoCoordinate(traveller.Location.Lat, traveller.Location.Lon);
                recommendations = recommendations.Where(user => travellerLoc.GetDistanceTo(new GeoCoordinate(user.Location.Lat, user.Location.Lon)) <= traveller.PrefProximity);

                //reset location update trigger
                firebase.Child("LocationUpdateTrigger").PutAsync(false);
            }

            if(!recommendations.Any())
            {
                return NotFound();
            }

            List<UserProfile> recommendedProfiles = new List<UserProfile>();
            foreach(var user in recommendations)
            {
                recommendedProfiles.Add(user.getProfile()); 
            }

            return Ok(recommendedProfiles);
        }
    }
}