using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpFireStarter
{
    public class FireBaseDB
    {
        //Stored Variables
        private string appID { get; set; }
        private string webAPIKey { get; set; }
        private string oAuthToken { get; set; }


        /// <summary>
        /// Initialize FirebaseConnector with AppID and WebAPI Key (Found on your Firebase Console Online)
        /// </summary>
        /// <param name="appID"></param>
        public FireBaseDB(string appID, string webAPIKey)
        {
            this.appID = appID;
            this.webAPIKey = webAPIKey;

            Log("Initialized with AppID " + appID);
        }



        /// <summary>
        /// Authenticate user and Obtain oAuth Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async void Authenticate(string email, string password)
        {
            //Validate Request
            if (email == string.Empty)
                Log("Email for Auth not provided.");
            else if(password == string.Empty)
                Log("Password for Auth not provided.");
            else if (webAPIKey == string.Empty)
                Log("WebAPI Key for Auth not provided.");

            string value = await Activity.Auth.Authenticate(email, password, webAPIKey);
            
            if(value != string.Empty && value != "")
            {
                oAuthToken = value;
                Log("oAuth Token Received: " + oAuthToken);
            }
            else
            {
                Log("Failed to Authenticate. Check your Username and Password.");
            }
        }


        /// <summary>
        /// Log Out Messages to Console
        /// </summary>
        /// <param name="message"></param>
        private static void Log(string message)
        {
            Console.WriteLine("SharpFireStarter: " + message);
        }

    }
}
