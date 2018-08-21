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
            if (!Uri.IsWellFormedUriString(appID, UriKind.RelativeOrAbsolute))
                throw new UriFormatException("The given AppID URL Structure is not valid");

            this.appID = appID;
            this.webAPIKey = webAPIKey;

            Logger.Log("Initialized with AppID " + appID);
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
            {
                Logger.Log("Email for Auth not provided.");
                return;
            }
            else if (password == string.Empty)
            {
                Logger.Log("Password for Auth not provided.");
                return;
            }
            else if (webAPIKey == string.Empty)
            {
                Logger.Log("WebAPI Key for Auth not provided.");
                return;
            }

            string value = await Activity.Auth.Authenticate(email, password, webAPIKey);

            if (value != string.Empty && value != "")
            {
                oAuthToken = value;
                Logger.Log("oAuth Token Received: " + oAuthToken);
            }
            else
            {
                Logger.Log("Failed to Authenticate. Check your Username and Password.");
            }
            
        }


        /// <summary>
        /// Write to the Firebase DB
        /// </summary>
        /// <param name="data"></param>
        public void WriteToDB(string data)
        {
            Activity.Set.WriteToDB(appID, "/todos", oAuthToken, "");
        }
    }
}
