using SharpFireStarter.Activity;
using SharpFireStarter.Models;
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

        public string databaseURL { get; set; }
        public string webAPIKey { get; set; }
        public string oAuthToken { get; set; }
        public string refreshToken { get; set; }
        public User currentUser { get; set; }


        public FireBaseDB()
        {

        }
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

        public User AuthenticateOAuth(string OAuthCode)
        {
            //Validate Request
            if (OAuthCode == null)
            {
                Logger.Log("OAuth Token for Auth not provided.");
                return null;
            }

            User user = Activity.Auth.AuthenticateOAuth(OAuthCode, webAPIKey);

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

        /// <summary>
        /// Update the user's display name in Firebase Auth
        /// </summary>
        /// <param name="idToken">The user's current ID token</param>
        /// <param name="displayName">The new display name to set</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateDisplayName(string idToken, string displayName)
        {
            return Activity.Auth.UpdateDisplayName(idToken, displayName, webAPIKey);
        }

        /// <summary>
        /// Update the user's display name in Firebase Auth (async version)
        /// </summary>
        /// <param name="idToken">The user's current ID token</param>
        /// <param name="displayName">The new display name to set</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> UpdateDisplayNameAsync(string idToken, string displayName)
        {
            return await Activity.Auth.UpdateDisplayNameAsync(idToken, displayName, webAPIKey);
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
        public bool WriteToDB(string node, object data, bool runAuthenticated = true)
        {
            try
            {
                Activity.Set.WriteToDB(databaseURL, node, oAuthToken, data, runAuthenticated, true);
                return true;
            } 
            catch(Exception ex)
            {
                // Check if it's a token expiration error and retry with refresh
                if (IsTokenExpiredError(ex) && !string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(webAPIKey))
                {
                    Logger.Log("Token expired during write, attempting refresh and retry...");
                    if (RefreshTokenInternal())
                    {
                        try
                        {
                            Logger.Log("Retrying write with refreshed token...");
                            Activity.Set.WriteToDB(databaseURL, node, oAuthToken, data, runAuthenticated, true);
                            return true;
                        }
                        catch (Exception retryEx)
                        {
                            Console.WriteLine("Write to DB retry after token refresh failed: " + retryEx.Message);
                            return false;
                        }
                    }
                }

                Console.WriteLine("Write to DB Failed: " + ex.Message);
                return false;
            }
        }

        public bool WriteToDB(string node, object data, bool runAuthenticated, bool silent = true)
        {
            try
            {
                Activity.Set.WriteToDB(databaseURL, node, oAuthToken, data, runAuthenticated, silent);
                return true;
            }
            catch (Exception ex)
            {
                // Check if it's a token expiration error and retry with refresh
                if (IsTokenExpiredError(ex) && !string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(webAPIKey))
                {
                    Logger.Log("Token expired during write, attempting refresh and retry...");
                    if (RefreshTokenInternal())
                    {
                        try
                        {
                            Logger.Log("Retrying write with refreshed token...");
                            Activity.Set.WriteToDB(databaseURL, node, oAuthToken, data, runAuthenticated, silent);
                            return true;
                        }
                        catch (Exception retryEx)
                        {
                            Console.WriteLine("Write to DB retry after token refresh failed: " + retryEx.Message);
                            return false;
                        }
                    }
                }

                Console.WriteLine("Write to DB Failed: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Set (replace) a node in Firebase DB - uses PUT instead of PATCH
        /// Use this when you want to completely replace a node rather than merge
        /// </summary>
        public bool SetToDB(string node, object data, bool runAuthenticated = true)
        {
            try
            {
                Activity.Set.SetToDB(databaseURL, node, oAuthToken, data, runAuthenticated, true);
                return true;
            }
            catch (Exception ex)
            {
                // Check if it's a token expiration error and retry with refresh
                if (IsTokenExpiredError(ex) && !string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(webAPIKey))
                {
                    Logger.Log("Token expired during set, attempting refresh and retry...");
                    if (RefreshTokenInternal())
                    {
                        try
                        {
                            Logger.Log("Retrying set with refreshed token...");
                            Activity.Set.SetToDB(databaseURL, node, oAuthToken, data, runAuthenticated, true);
                            return true;
                        }
                        catch (Exception retryEx)
                        {
                            Console.WriteLine("Set to DB retry after token refresh failed: " + retryEx.Message);
                            return false;
                        }
                    }
                }

                Console.WriteLine("Set to DB Failed: " + ex.Message);
                return false;
            }
        }

        //############################  GET DATA FROM DB ############################

        public string GetFromDB(string data)
        {
            return runGetDataActivity(databaseURL, data, true, null, null, null, null, 0, 0, false);
        }
        public string GetFromDB(string data, bool runAuthenticated)
        {
            return runGetDataActivity(databaseURL, data, runAuthenticated, null, null, null, null, 0, 0, false);
        }
        public string GetFromDB(string data, bool runAuthenticated, bool shallow)
        {
            return runGetDataActivity(databaseURL, data, runAuthenticated, null, null, null, null, 0, 0, shallow);
        }

        public string GetFromDB(string data, bool runAuthenticated, string orderBy)
        {
            return runGetDataActivity(databaseURL, data, runAuthenticated, orderBy, null, null, null, 0, 0, false);
        }

        public string GetFromDB(string data, bool runAuthenticated, string orderBy, string startAt, string endAt = null, string equalTo = null, int limitToFirst = 0, int limitToLast = 0)
        {
            return runGetDataActivity(databaseURL, data, runAuthenticated, orderBy, startAt, endAt, equalTo, limitToFirst, limitToLast, false);            
        }


        private string runGetDataActivity(string databaseURL, string data, bool runAuthenticated = true, string orderBy = null, string startAt = null, string endAt = null, string equalTo = null, int limitToFirst = 0, int limitToLast = 0, bool shallow = false)
        {
            try
            {
                string getData = Activity.Get.GetFromDB(databaseURL, data, oAuthToken, runAuthenticated, orderBy, startAt, endAt, equalTo, limitToFirst, limitToLast, shallow);
                if (getData == "" || getData.ToLower() == "null")
                    return null;

                return getData;
            }
            catch (Exception ex)
            {
                // Check if it's a token expiration error and retry with refresh
                if (IsTokenExpiredError(ex) && !string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(webAPIKey))
                {
                    Logger.Log("Token expired, attempting refresh and retry...");
                    if (RefreshTokenInternal())
                    {
                        try
                        {
                            Logger.Log("Retrying request with refreshed token...");
                            string getData = Activity.Get.GetFromDB(databaseURL, data, oAuthToken, runAuthenticated, orderBy, startAt, endAt, equalTo, limitToFirst, limitToLast, shallow);
                            if (getData == "" || getData.ToLower() == "null")
                                return null;

                            return getData;
                        }
                        catch (Exception retryEx)
                        {
                            Logger.Log("Retry after token refresh failed: " + retryEx.Message);
                            return null;
                        }
                    }
                }

                Logger.Log(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Check if the exception indicates a token expiration
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsTokenExpiredError(Exception ex)
        {
            if (ex == null) return false;
            
            string message = ex.Message.ToLower();
            return message.Contains("401") || 
                   message.Contains("unauthorized") || 
                   message.Contains("token expired") || 
                   message.Contains("invalid token") ||
                   message.Contains("permission denied");
        }

        /// <summary>
        /// Refresh the current token using the stored refresh token
        /// </summary>
        /// <returns></returns>
        private bool RefreshTokenInternal()
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(webAPIKey))
                {
                    Logger.Log("Cannot refresh token: refresh token or webAPIKey missing");
                    return false;
                }

                // Create a temporary user object for the refresh operation
                var tempUser = new User { refreshToken = this.refreshToken };
                
                var refreshResult = Activity.Auth.RefreshAuthToken(ref tempUser, webAPIKey);
                
                if (refreshResult != null && !string.IsNullOrEmpty(refreshResult.id_token))
                {
                    // Update stored tokens
                    this.oAuthToken = refreshResult.id_token;
                    this.refreshToken = refreshResult.refresh_token;
                    
                    // Update current user if exists
                    if (currentUser != null)
                    {
                        currentUser.idToken = refreshResult.id_token;
                        currentUser.refreshToken = refreshResult.refresh_token;
                    }
                    
                    Logger.Log("Token refreshed successfully");
                    return true;
                }
                else
                {
                    Logger.Log("Token refresh failed: no valid response");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Token refresh failed: " + ex.Message);
                return false;
            }
        }
    }
}
