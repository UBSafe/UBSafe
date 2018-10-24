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
        static string auth = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita"; // app secret
        FirebaseClient firebase = new FirebaseClient(
          "https://ubsafe-a816e.firebaseio.com/",
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => Task.FromResult(auth)
          });

        // GET: api/Users
        [HttpGet]
        public List<User> Get()
        {
            List<User> allUsers = firebase.Child("Users").OnceSingleAsync<Dictionary<string, User>>().Result.Values.ToList();
            return allUsers;
        }

        // GET: api/Users/5
        [HttpGet("{userID}", Name = "Get")]
        public User Get(string userID)
        {
            User user = firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;
            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async void Post(string userID, string userName, int age, string gender, float lat, float lon, int prefAgeMin, int prefAgeMax, float prefProximity, bool femaleCompanionsOkay, bool maleCompanionsOkay, bool otherCompanionsOkay)
        {
            User newUser = CreateTestUser(userID, userName, age, gender, prefAgeMin, prefAgeMax, prefProximity, femaleCompanionsOkay, maleCompanionsOkay, otherCompanionsOkay);
            await firebase.Child("Users/" + userID).PutAsync(newUser);
        }

        /* PUT: api/Users/5
         * 
         * API endpoint for updating a user's info in the db. 
         * Note: Location cannot be updated through this endpoint. 
         * All location updates should be done on the client. 
         */
        [HttpPut("{userID}")]
        public void Put([FromRoute] string userID, [FromBody] Preference newPreferences)  
        {
            User oldUser = firebase.Child("Users").Child(userID).OnceSingleAsync<User>().Result;

            var UserName = oldUser.UserName;
            var Age = oldUser.Age;
            var Gender = oldUser.Gender;
            var PrefAgeMin = newPreferences.PrefAgeMin;
            var PrefAgeMax = newPreferences.PrefAgeMax;
            var PrefProximity = newPreferences.Proximity;
            var FemaleCompanionsOkay = newPreferences.FemaleCompanionsOkay;
            var MaleCompanionsOkay = newPreferences.MaleCompanionsOkay;
            var OtherCompanionsOkay = newPreferences.OtherCompanionsOkay;
            var Location = oldUser.Location;

            var updatedUser = new User(userID, UserName, Age, Gender, PrefAgeMin, PrefAgeMax, PrefProximity, FemaleCompanionsOkay, MaleCompanionsOkay, OtherCompanionsOkay, Location);

            firebase.Child("Users").Child(userID).PutAsync(updatedUser);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userID}")]
        public async void Delete([FromRoute] string userID)
        {
            await firebase
              .Child("Users")
              .Child(userID)
              .DeleteAsync();
        }

        public static User CreateTestUser(string userID, string userName, int age, string gender, int prefAgeMin, int prefAgeMax, float prefProximity, bool femaleCompanionsOkay, bool maleCompanionsOkay, bool otherCompanionsOkay)
        {
            return new User(userID, userName, age, gender, prefAgeMin, prefAgeMax, prefProximity, femaleCompanionsOkay, maleCompanionsOkay, otherCompanionsOkay, new Location(-1, -1, DateTime.Now));
        }
    }
}
