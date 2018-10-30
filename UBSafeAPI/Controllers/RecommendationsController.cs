using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;
using UBSafeAPI.Models.SideGeoFire;

using Microsoft.AspNetCore.Authorization;
using Firebase.Database;
using Firebase.Database.Query;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        static readonly string Auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita";
        static readonly string Db = "https://ubsafe-a816e.firebaseio.com/";
        static readonly FirebaseClient Firebase = new FirebaseClient(
          Db,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(Auth)
          });

        // GET: api/Recommendations
        [HttpGet("{userID}", Name = "GetRecommendations")]
        public ActionResult Get([FromRoute] string userID)
        {
            User traveller;
            IEnumerable<User> recommendations;
            List<UserProfile> recommendedProfiles = new List<UserProfile>();

            try
            {
                /*
                 * trigger location update in db - 
                 * we do this before issuing other queries to give the 
                 * clients time to update their locations in the db
                 */
                Firebase.Child("LocationUpdateTrigger").PutAsync(true);

                //Get current user's preferences for querying + filtering purposes
                traveller = Firebase
                            .Child("Users")
                            .Child(userID)
                            .OnceSingleAsync<User>()
                            .Result;

                if(traveller == null)
                {
                    return BadRequest(new { statusCode = 400, errorMessage = "User does not exist in the database.", responseData = ""});
                }


                GeoQuery geoQuery = new GeoQuery(traveller.Location.Lat, traveller.Location.Lon, traveller.Preferences.Proximity);


                /*
                 * Pull the first 100 users that are within the user's 
                 * preferred proximity  
                 */
                var returnedRecommendations = Firebase
                                            .Child("Users")
                                            .OrderBy("Geohash")
                                            .StartAt(geoQuery.lowerGeoHash)
                                            .EndAt(geoQuery.upperGeoHash)
                                            .LimitToFirst(100)
                                            .OnceSingleAsync<Dictionary<string, User>>()
                                            .Result;

                //reset location update trigger
                Firebase.Child("LocationUpdateTrigger").PutAsync(false);

                if(returnedRecommendations == null)
                {
                    return NotFound(new { statusCode = 400, errorMessage = "No matching Virtual Companions found/available.", responseData = recommendedProfiles});
                }

                // remove current user from list so that they don't get themselves as a recommendation
                returnedRecommendations.Remove(userID);

                //we received a json object with userID's as keys and users as values - we only 
                //need users now, so remove the keys
                recommendations = returnedRecommendations.Values.ToList();

                /* filter based on the user's remaining preferences  - note that this is done on 
                 * the server instead of in the initial query because the 
                 * Firebase realtime database does not support compounded queries
                 */
                recommendations = recommendations.Where(user => user.Age >= traveller.Preferences.AgeMin && user.Age <= traveller.Preferences.AgeMax);

                if (!traveller.Preferences.MaleCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Male");
                }
                if (!traveller.Preferences.FemaleCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Female");
                }
                if (!traveller.Preferences.OtherCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Other");
                }

                recommendations = recommendations.OrderBy(user => user.Location.LastUpdated);

                if(!recommendations.Any())
                {
                    return NotFound(new { statusCode = 400, errorMessage = "No matching Virtual Companions found.", responseData = recommendedProfiles});
                }

                foreach(var user in recommendations)
                {
                    recommendedProfiles.Add(user.GetProfile()); 
                }

                return Ok(new { statusCode = 200, errorMessage = "", responseData = recommendedProfiles});
            }

            catch(Exception e)
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = e.Message, responseData = ""});
            }
        }
    }
}