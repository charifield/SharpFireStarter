using SharpFireStarter.Activity;
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

        private string databaseURL { get; set; }
        private string webAPIKey { get; set; }
        private string oAuthToken { get; set; }
        public User currentUser { get; set; }


        /// <summary>
        /// Initialize FirebaseConnector with AppID and WebAPI Key (Found on your Firebase Console Online)
        /// </summary>
        /// <param name="appID"></param>
        public FireBaseDB(string appID, string databaseURL, string webAPIKey)
        {
            if (!Uri.IsWellFormedUriString(appID, UriKind.RelativeOrAbsolute))
                throw new UriFormatException("The given AppID URL Structure is not valid");

            this.databaseURL = databaseURL;
            this.webAPIKey = webAPIKey;

            Logger.Log("Initialized with AppID " + appID);
        }



        /// <summary>
        /// Authenticate user and Obtain oAuth Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string email, string password)
        {
            //Validate Request
            if (email == string.Empty)
            {
                Logger.Log("Email for Auth not provided.");
                return null;
            }
            else if (password == string.Empty)
            {
                Logger.Log("Password for Auth not provided.");
                return null;
            }
            else if (webAPIKey == string.Empty)
            {
                Logger.Log("WebAPI Key for Auth not provided.");
                return null;
            }

            User user = Activity.Auth.Authenticate(email, password, webAPIKey);

            if (user != null)
            {
                currentUser = user;
                oAuthToken = user.idToken;
                Logger.Log("oAuth Token Received: " + oAuthToken);
                return user;
            }
            else
            {
                Logger.Log("Failed to Authenticate. Check your Username and Password.");
                return null;
            }
            
        }

        public User SignUp(string name, string email, string password)
        {
            var newUser = Auth.SignUp(name, email, password, webAPIKey);
            if (newUser != null)
                return newUser;
            else
                return null;
        }

        public bool SignOut()
        {
            if(currentUser != null)
            {
                Auth.SignOut(currentUser, webAPIKey);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Write to the Firebase DB
        /// </summary>
        /// <param name="data"></param>
        public bool WriteToDB(string node, object data)
        {
            try
            {
                Activity.Set.WriteToDB(databaseURL, node, oAuthToken, data);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Write to DB Failed: " + ex.Message);
                return false;
            }
            return true;
        }



        /// <summary>
        /// Get Data from the Firebasse DB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetFromDB(string data)
        {
            try
            {
                string getData = Activity.Get.GetFromDB(databaseURL, data, oAuthToken);
                if (getData == "" || getData.ToLower() == "null")
                    return null;

                return getData;
            } catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
        }
    }
}
