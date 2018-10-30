using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Authorization;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        static readonly string Auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita";
        static readonly string Db = "https://ubsafe-a816e.firebaseio.com/";
        static readonly FirebaseClient Firebase = new FirebaseClient(
          Db,
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(Auth)
          });

        // GET: api/Users
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                List<User> allUsers = Firebase.Child("Users").OnceSingleAsync<Dictionary<string, User>>().Result.Values.ToList();
                if(allUsers == null)
                {
                    return BadRequest(new { statusCode = 400, errorMessage = "Error: No users found in the database.", responseData = ""});
                }
                else
                {
                    return Ok(new { statusCode = 200, errorMessage = "", responseData = allUsers});
                }
            }
            catch(Exception e)
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = e.Message, responseData = ""});
            }
        }

        // GET: api/Users/5
        [HttpGet("{userID}", Name = "Get")]
        public ActionResult Get(string userID)
        {
            try
            {
                User user = Firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;
                if(user == null)
                {
                    return BadRequest(new { statusCode = 400, errorMessage = "Error: User does not exist in the database.", responseData = ""});
                }
                else
                {
                    return Ok(new { statusCode = 200, errorMessage = "", responseData = user});
                }
            }
            catch(Exception e)
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = e.Message, responseData = ""});
            }
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult Post( [FromBody] User user)
        {
            var response = Firebase.Child("Users/" + user.UserID).PutAsync(user);
            if (response.IsCompletedSuccessfully)
            {
                return Ok(new {statusCode = 200, errorMessage = "", responseData = user});
            }
            else
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = "Failed to create user.", responseData = user});

            }
        }

        /* PUT: api/Users/5
         * 
         * API endpoint for updating a user's info in the db. 
         * Note: Location cannot be updated through this endpoint. 
         * All location updates should be done on the client. 
         */
        [HttpPut("{userID}")]
        public ActionResult Put([FromRoute] string userID, [FromBody] Preference newPreferences)  
        {
            User user;

            try
            {
                user = Firebase
                            .Child("Users")
                            .Child(userID)
                            .OnceSingleAsync<User>()
                            .Result;

                user.Preferences.AgeMin = (newPreferences.AgeMin == -1)? user.Preferences.AgeMin : newPreferences.AgeMin;
                user.Preferences.AgeMax = (newPreferences.AgeMax == -1)? user.Preferences.AgeMax : newPreferences.AgeMax;
                user.Preferences.Proximity = (newPreferences.Proximity == -1)? user.Preferences.Proximity : newPreferences.Proximity;
                user.Preferences.FemaleCompanionsOkay = newPreferences.FemaleCompanionsOkay;
                user.Preferences.MaleCompanionsOkay = newPreferences.MaleCompanionsOkay;
                user.Preferences.OtherCompanionsOkay = newPreferences.OtherCompanionsOkay;

                Firebase.Child("Users").Child(userID).PutAsync(user);

                return Ok(new { statusCode = 200, errorMessage = "", responseData = user});
            }
            catch(Exception e)
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = "Failed to create user.", responseData = e.Message});
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userID}")]
        public ActionResult Delete([FromRoute] string userID)
        {
            try
            {
               var response = Firebase.Child("Users").Child(userID).DeleteAsync();
               return Ok(new { statusCode = 200, errorMessage = "", responseData = "Successfully deleted user" + userID});
            }
            catch(Exception e)
            {
                return StatusCode(504, new { statusCode = 504, errorMessage = "Failed to delete user.", responseData = e.Message});
            }
        }
    }
}
