using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;
using UBSafeAPI.GeoQuerying;

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
        static readonly string auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita";
        static readonly string dbUrl = "https://ubsafe-a816e.firebaseio.com/";

        FirebaseClient firebase = new FirebaseClient(
          dbUrl,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(auth)
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
                //Get current user's preferences for querying + filtering purposes
                traveller = firebase
                            .Child("Users")
                            .Child(userID)
                            .OnceSingleAsync<User>()
                            .Result;

                if(traveller == null)
                {
                    return BadRequest(new { isSuccess = false, statusCode = 400, errorMessage = "User does not exist in the database."});
                }

                /*
                 * trigger location update in db - 
                 * we do this before issuing other queries to give the 
                 * clients time to update their locations in the db
                 */
                firebase.Child("LocationUpdateTrigger").PutAsync(true);

                GeoQuery geoQuery = new GeoQuery(traveller.Location.Lat, traveller.Location.Lon, traveller.PrefProximity);

                var returnedRecommendations = firebase
                                            .Child("Users")
                                            .OrderBy("Geohash")
                                            .StartAt(geoQuery.LowerGeoHash)
                                            .EndAt(geoQuery.UpperGeoHash)
                                            .LimitToFirst(100)
                                            .OnceSingleAsync<Dictionary<string, User>>()
                                            .Result;

                // remove current user from list so that they don't get themselves as a recommendation
                returnedRecommendations.Remove(userID);

                //we received a json object with userID's as keys and users as values - we only 
                //need users now, so remove the keys
                recommendations = returnedRecommendations.Values.ToList();

                if (!recommendations.Any())
                {
                    return NotFound(new { isSuccess = false, statusCode = 400, message = "No matching Virtual Companions found/available.", responseData = recommendedProfiles});
                }

                //filter based on the user's remaining preferences  - note that this is done on 
                //the server instead of in the initial query because the 
                //firebase realtime database does not support compounded queries
                recommendations = recommendations.Where(user => user.Age >= traveller.PrefAgeMin && user.Age <= traveller.PrefAgeMax);

                if (!traveller.MaleCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Male");
                }
                if (!traveller.FemaleCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Female");
                }
                if (!traveller.OtherCompanionsOkay)
                {
                    recommendations = recommendations.Where(user => user.Gender != "Other");
                }

                //reset location update trigger
                firebase.Child("LocationUpdateTrigger").PutAsync(false);

                if(!recommendations.Any())
                {
                    return NotFound(new { isSuccess = false, statusCode = 400, message = "No matching Virtual Companions found.", responseData = recommendedProfiles});
                }

                foreach(var user in recommendations)
                {
                    recommendedProfiles.Add(user.GetProfile()); 
                }

                return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = recommendedProfiles});
                }

            catch(Exception e)
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = e.Message, responseData = ""});
            }
        }
    }
}