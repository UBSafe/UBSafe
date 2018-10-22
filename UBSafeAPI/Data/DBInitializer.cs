using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBSafeAPI.Data;
using UBSafeAPI.Models;

namespace UBSafeAPI.Data
{
    public class DBInitializer
    {
        public static void Initialize(UBSafeContext context)
        {
            /*

            //look for any users
            if (context.Users.Any())
            {
                //DB has been seeded
                return;
            }

            var userList = new List<User>
            {
                new User{LastName="Kirby", FirstName="Lisa", Age=21, Gender = Gender.Female },
                new User{LastName="Magbanua", FirstName="Andrea", Age=21, Gender = Gender.Female },
                new User{LastName="Mollitor", FirstName="Cormac", Age=21, Gender = Gender.Male },
                new User{LastName="Elford", FirstName="Marshall", Age=21, Gender = Gender.Male },
                new User{LastName="Test", FirstName="Test", Age=100, Gender = Gender.Other },
            };
            context.Users.AddRange(userList);
            context.SaveChanges();
            */
        }
    }
}
