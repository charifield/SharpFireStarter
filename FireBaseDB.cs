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

            User user = Activity.Auth.Authenticate(email.Trim(), password.Trim(), webAPIKey);

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

        public void SetoAuthToken(string token)
        {
            this.oAuthToken = token;
        }

        public User ChangePassword(string email, string oldpassword, string newpassword)
        {
            //Validate Request
            if (email == string.Empty)
            {
                Logger.Log("Email for Auth not provided.");
                return null;
            }
            else if (oldpassword == string.Empty)
            {
                Logger.Log("Old Password for Auth not provided.");
                return null;
            }
            else if (newpassword == string.Empty)
            {
                Logger.Log("New Password for Auth not provided.");
                return null;
            }

            User user = Activity.Auth.Authenticate(email.Trim(), oldpassword.Trim(), webAPIKey);

            if (user != null)
            {
                User userChangedPassword = Activity.Auth.ChangePassword(ref user, email.Trim(), oldpassword.Trim(), newpassword.Trim(), webAPIKey);
                Logger.Log("Password has been changed");
                return user;
            }
            else
            {
                Logger.Log("Failed to Authenticate. Check your old Username and Password.");
                return null;
            }

        }

        public bool ChangePasswordWithOOBCode(string resetCode, string newPassword)
        {
            //Validate Request
            if (resetCode == string.Empty)
            {
                Logger.Log("Reset code was not proved.");
                return false;
            }

            if (newPassword == string.Empty)
            {
                Logger.Log("New password was not proved.");
                return false;
            }

            string passwordResetWithCode = Activity.Auth.ChangePasswordWithOOBCode(resetCode, newPassword, webAPIKey);

            if (passwordResetWithCode != null && passwordResetWithCode != "expired")
            {
                Logger.Log("Password was successfully changed");
                return true;
            }
            else
            {
                Logger.Log("Password reset code was invalid or expired");
                return false;
            }
        }

        public bool SendPasswordResetEmail(string email)
        {
            //Validate Request
            if (email == string.Empty)
            {
                Logger.Log("Email for Auth not provided.");
                return false;
            }

            bool sendResetEamil = Activity.Auth.ResetPassword(email.Trim(), webAPIKey);

            if (sendResetEamil == true)
            {
                Logger.Log("Password reset email has been sent");
                return sendResetEamil;
            }
            else
            {
                Logger.Log("Failed to Send Email Reset Password. Check your email address.");
                return sendResetEamil;
            }
        }

        public string VerifyPasswordResetCode(string resetCode)
        {
            //Validate Request
            if (resetCode == string.Empty)
            {
                Logger.Log("Reset code was not proved.");
                return null;
            }

            string passwordResetCodeEmail = Activity.Auth.VerifyPasswordResetCode(resetCode, webAPIKey);

            if (passwordResetCodeEmail != null && passwordResetCodeEmail != "expired")
            {
                Logger.Log("Password reset code was valid");
                return passwordResetCodeEmail;
            }
            else
            {
                Logger.Log("Password reset code was invalid or expired");
                return "expired";
            }
        }

        public bool SendPasswordResetEmail(string email, string newPassword)
        {
            //Validate Request
            if (email == string.Empty)
            {
                Logger.Log("Email for Auth not provided.");
                return false;
            }

            bool sendResetEamil = Activity.Auth.ResetPassword(email.Trim(), webAPIKey);

            if (sendResetEamil == true)
            {
                Logger.Log("Password reset email has been sent");
                return sendResetEamil;
            }
            else
            {
                Logger.Log("Failed to Send Email Reset Password. Check your email address.");
                return sendResetEamil;
            }

        }

        /// <summary>
        /// Sign up new user
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User SignUp(string name, string email, string password)
        {
            var newUser = Auth.SignUp(name.Trim(), email.Trim(), password.Trim(), webAPIKey);
            if (newUser != null)
            {
                currentUser = newUser;
                oAuthToken = newUser.idToken;
                return newUser;
            }
                
            else
                return null;
        }

        /// <summary>
        /// Sign Out Logged In Account
        /// </summary>
        /// <returns></returns>
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
