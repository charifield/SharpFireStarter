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
        public FireBaseDB(string appURL, string appID, string webAPIKey)
        {
            if (!Uri.IsWellFormedUriString(appID, UriKind.RelativeOrAbsolute))
                throw new UriFormatException("The given AppID URL Structure is not valid");

            this.appID = appURL + "/" + appID + "/";
            this.webAPIKey = webAPIKey;

            Logger.Log("Initialized with AppID " + appID);
        }



        /// <summary>
        /// Authenticate user and Obtain oAuth Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string email, string password)
        {
            //Validate Request
            if (email == string.Empty)
            {
                Logger.Log("Email for Auth not provided.");
                return false;
            }
            else if (password == string.Empty)
            {
                Logger.Log("Password for Auth not provided.");
                return false;
            }
            else if (webAPIKey == string.Empty)
            {
                Logger.Log("WebAPI Key for Auth not provided.");
                return false;
            }

            string value = Activity.Auth.Authenticate(email, password, webAPIKey);

            if (value != string.Empty && value != "")
            {
                oAuthToken = value;
                Logger.Log("oAuth Token Received: " + oAuthToken);
                return true;
            }
            else
            {
                Logger.Log("Failed to Authenticate. Check your Username and Password.");
                return false;
            }
            
        }


        /// <summary>
        /// Write to the Firebase DB
        /// </summary>
        /// <param name="data"></param>
        public bool WriteToDB(string node, object data)
        {
            Activity.Set.WriteToDB(appID, node, oAuthToken, data);
            return true;
        }



        /// <summary>
        /// Get Data from the Firebasse DB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetFromDB(string data)
        {
            string getData = Activity.Get.GetFromDB(appID, data, oAuthToken);
            return getData;
        }
    }
}
