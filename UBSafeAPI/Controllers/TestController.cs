using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;
using UBSafeAPI.Data;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita",
            BasePath = "https://ubsafe-a816e.firebaseio.com/"
        };

        IFirebaseClient client; 

        //private readonly UBSafeContext db;
        //public TestController(UBSafeContext _db)
        //{
        //    db = _db; 
        //}
        
        // GET api/test
        [HttpGet]
        public async Task<List<User>> Get()
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            FirebaseResponse response = await client.GetAsync("Users/");
            List<User> allUsers = response.ResultAs<List<User>>();
            return allUsers;
        }

        // GET api/test/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/test
        [HttpPost]
        public async void Post(int userID, int age, string firstName, string lastName, Gender gender)
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            var user = new User
            {
                UserID = userID,
                Age = age,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender 
            };
            SetResponse response = await client.SetAsync("Users/" + user.UserID, user);
            User result = response.ResultAs<User>();
            Console.WriteLine(result);
        }

        // PUT api/test/5
        [HttpPut("{id}")]
        public async void Put(int id, string name="", int age=-1, string gender="", int prefMinAge=-1, int prefMaxAge=-1, string prefGender="", float prefProximity=-1)
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            FirebaseResponse response = await client.GetAsync("Users/" + id);
            User oldUser = response.ResultAs<User>();


            var user = new User
            {
            };

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
