using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UBSafeAPI.Models;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp;

namespace UBSafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uieSwdqrzXirqrSoJk55xGitX7dsr85fkaps5Ita",
            BasePath = "https://ubsafe-a816e.firebaseio.com/"
        };

        IFirebaseClient client; 

        // GET: api/Users
        [HttpGet]
        public async Task<List<User>> Get()
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            FirebaseResponse response = await client.GetAsync("Users/", QueryBuilder.New().OrderBy("Age"));
            List<User> allUsers = response.ResultAs<List<User>>();
            return allUsers;

        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<User> Get(string userID)
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            FirebaseResponse response = await client.GetAsync("Users/" + userID);
            User user = response.ResultAs<User>();

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async void Post(int userID, int age, string firstName, string lastName, string gender)
        {
            client = new FireSharp.FirebaseClient(config);
            if(client != null)
            {
                Console.WriteLine("Connection established.");
            }

            var user = new User
            {
            };
            SetResponse response = await client.SetAsync("Users/", user);
            User result = response.ResultAs<User>();
            Console.WriteLine(result);
        }

        // PUT: api/Users/5
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
