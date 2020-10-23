using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFireStarter
{
    public class User
    {
        public string email { get; set; }
        public string displayName { get; set; }
        public string oauthAccessToken { get; set; }
        public string photoUrl { get; set; }
        public float oauthExpireIn { get; set; }
        public bool registered { get; set; }
        public string idToken { get; set; }

        //Used for loggin in and out
        public string localId { get; set; }
        public string instanceId { get; set; }
    }
}
