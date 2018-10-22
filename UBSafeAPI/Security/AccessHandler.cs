using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Newtonsoft.Json;

namespace UBSafeAPI.Security
{
    public class AccessRequirement : IAuthorizationRequirement
    {
    }

    public class AccessHandler : AuthorizationHandler<AccessRequirement>
    {

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement)
        {
            var user = context.User;

            if (user != null && user.Claims != null)
            {
                var firebaseClaim = user.Claims.FirstOrDefault(c => c.Type == "firebase");
                FirebaseUserInfo firebaseUserInfo = null;

                if (firebaseClaim != null && firebaseClaim.Value != null)
                {
                    firebaseUserInfo = JsonConvert.DeserializeObject<FirebaseUserInfo>(firebaseClaim.Value);

                    if (firebaseUserInfo != null)
                        Debug.WriteLine(firebaseUserInfo.SignInProvider);

                    // do some custom checks: call context.Succeed() if user is OK
                    context.Succeed(requirement);
                }

            }


        }
    }
}
