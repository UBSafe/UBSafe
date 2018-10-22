//Credit to:  http://dvit.eu/frank/2017/04/01/firebase-authentication-with-an-asp-net-core-backend-in-azure/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace UBSafeAPI.Security
{
    public class FirebaseIdentities
    {
        [JsonProperty(PropertyName = "facebook.com")]
        public string[] FacebookDotCom { get; set; }

        [JsonProperty(PropertyName = "google.com")]
        public string[] GoogleDotCom { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string[] Email { get; set; }
    }

    public class FirebaseUserInfo
    {
        [JsonProperty(PropertyName = "identities")]
        public FirebaseIdentities Identities { get; set; }

        [JsonProperty(PropertyName = "sign_in_provider")]
        public string SignInProvider { get; set; }
    }

}
