using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UBSafeAPI.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        static readonly string auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita"; // app secret
        FirebaseClient firebase = new FirebaseClient(
          "https://ubsafe-a816e.firebaseio.com/",
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(auth)
          });

        // GET: api/Users
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                List<User> allUsers = firebase.Child("Users").OnceSingleAsync<Dictionary<string, User>>().Result.Values.ToList();
                if(allUsers == null)
                {
                    return BadRequest(new { isSuccess = false, statusCode = 400, message = "Error: No users found in the database.", responseData = ""});
                }
                else
                {
                    return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = allUsers});
                }
            }
            catch(Exception e)
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = e.Message, responseData = ""});
            }
        }

        // GET: api/Users/5
        [HttpGet("{userID}", Name = "Get")]
        public ActionResult Get(string userID)
        {
            try
            {
                User user = firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;
                if(user == null)
                {
                    return BadRequest(new { isSuccess = false, statusCode = 400, message = "Error: User does not exist in the database.", responseData = ""});
                }
                else
                {
                    return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = user});
                }
            }
            catch(Exception e)
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = e.Message, responseData = ""});
            }
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult Post( [FromBody] User user)
        {
            var response = firebase.Child("Users/" + user.UserID).PutAsync(user);
            if (response.IsCompletedSuccessfully)
            {
                return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = user});
            }
            else
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = "Failed to create user.", responseData = user});

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
            User oldUser;
            int PrefAgeMin, PrefAgeMax;
            float PrefProximity;
            bool FemaleCompanionsOkay, MaleCompanionsOkay, OtherCompanionsOkay;

            try
            {
                oldUser = firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;

                PrefAgeMin = (newPreferences.PrefAgeMin == null)? oldUser.PrefAgeMin : (int) newPreferences.PrefAgeMin;
                PrefAgeMax = (newPreferences.PrefAgeMax == null)? oldUser.PrefAgeMax : (int) newPreferences.PrefAgeMax;
                PrefProximity = (newPreferences.Proximity == null)? oldUser.PrefProximity : (float) newPreferences.Proximity;
                FemaleCompanionsOkay = (newPreferences.FemaleCompanionsOkay == null)? oldUser.FemaleCompanionsOkay : (bool) newPreferences.FemaleCompanionsOkay;
                MaleCompanionsOkay = (newPreferences.MaleCompanionsOkay == null)? oldUser.MaleCompanionsOkay : (bool) newPreferences.MaleCompanionsOkay;
                OtherCompanionsOkay = (newPreferences.OtherCompanionsOkay == null)? oldUser.OtherCompanionsOkay : (bool) newPreferences.OtherCompanionsOkay;
                Location Location = oldUser.Location;

                var updatedUser = new User(userID, oldUser.UserName, oldUser.Age, oldUser.Gender, PrefAgeMin, PrefAgeMax, PrefProximity, FemaleCompanionsOkay, MaleCompanionsOkay, OtherCompanionsOkay, Location);
                firebase.Child("Users").Child(userID).PutAsync(updatedUser);
            return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = updatedUser});
            }
            catch(Exception e)
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = "Failed to create user.", responseData = e.Message});
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userID}")]
        public ActionResult Delete([FromRoute] string userID)
        {
            try
            {
               var response = firebase.Child("Users").Child(userID).DeleteAsync();
               return Ok(new { isSuccess = true, statusCode = 200, message = "", responseData = "Successfully deleted user" + userID});
            }
            catch(Exception e)
            {
                return StatusCode(504, new { isSuccess = false, statusCode = 504, message = "Failed to delete user.", responseData = e.Message});
            }
        }
    }
}
