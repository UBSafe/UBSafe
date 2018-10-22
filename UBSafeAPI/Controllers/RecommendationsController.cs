using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UBSafeAPI.Models;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita",
            BasePath = "https://ubsafe-a816e.firebaseio.com/"
        };

        IFirebaseClient client; 
        // GET: api/Recommendations
        [HttpGet]
        public List<User> Get(string userID)
        {
            FirebaseResponse triggerLocationUpdate;
            FirebaseResponse curUser;
            FirebaseResponse response;
            Preference preferences;
            List<User> recommendations;
            bool triggered;

            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            /*
             * Trigger location update in db - we do this before issuing other queries to give the 
             * clients time to update their locations in the db
             */
            triggerLocationUpdate = client.Update("/", new UpdateTrigger { LocationUpdateTrigger = false } );
            triggered = triggerLocationUpdate.ResultAs<UpdateTrigger>().LocationUpdateTrigger;
            Console.WriteLine(triggered);

            //Get current user's preferences for querying purposes
            curUser = client.Get("Users/" + userID);
            //preferences = curUser.ResultAs<User>().Preferences;

            /*NOTE: ASSUMES THAT AGEMIN AND AGEMAX ARE DEFINED*/
            //Query/filter by the current user's preferences
            response = client.Get("Users/", QueryBuilder.New().OrderBy("Age"));
            if (response.Body == "{}")
            {
                //TODO: return error response: No users found
                return new List<User>();
            }
            recommendations = response.ResultAs <List<User>>();
            //TODO: additional filtering based on remaining preferences

            return recommendations;
        }

        // GET: api/Recommendations/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Recommendations
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Recommendations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
