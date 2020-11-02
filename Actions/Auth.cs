using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpFireStarter.Models;

namespace SharpFireStarter.Activity
{
    public static class Auth
    {
        //ResourceScope : https://firebase.google.com/docs/reference/rest/auth#section-create-email-password
        //Second resource : https://any-api.com/googleapis_com/identitytoolkit/docs/relyingparty/identitytoolkit_relyingparty_signOutUser


        private static readonly string authEndPoint = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=";
        private static readonly string authRefreshTokenEndPoint = "https://securetoken.googleapis.com/v1/token?key=";

        private static readonly string signUpEndPoint = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
        private static readonly string signOutEndPoint = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signOutUser?key=";
        private static readonly string changePasswordEndPoint = "https://identitytoolkit.googleapis.com/v1/accounts:update?key=";

        private static readonly string resetPasswordEndPoint = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=";
        private static readonly string verifyResetPasswordEndPoint = "https://identitytoolkit.googleapis.com/v1/accounts:resetPassword?key=";


        /// <summary>
        /// Get oAuth Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        //public static Task<string> Authenticate(string email, string password, string webAPIKey)
        public static User Authenticate(string email, string password, string webAPIKey)
        {
            Log(string.Format("Begin Authentication for {0}...", email));
            var client = new HttpClient();
            var taskResult = new TaskCompletionSource<string>();
            taskResult.SetResult("");

            try
            {
                var authValues = new Dictionary<string, string>
                {
                    {"email", email },
                    {"password", password },
                    {"returnSecureToken", "true" },
                    {"instanceId", Guid.NewGuid().ToString() }
                };

                var content = new FormUrlEncodedContent(authValues);
                var response = client.PostAsync(authEndPoint + webAPIKey, content);
                response.Wait();
                var responseString = response.Result.Content.ReadAsStringAsync();
                responseString.Wait();

                if (responseString.Result != null && responseString.Result != "")
                {
                    var userObject = JsonConvert.DeserializeObject<User>(responseString.Result);
                    userObject.instanceId = authValues["instanceId"];

                    //Get User ID
                    var newToken = RefreshAuthToken(ref userObject, webAPIKey);
                    return userObject;
                }
                else
                    throw new UnauthorizedAccessException("Failed to obtain Token. Check Credentials");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }
            finally
            {
                client.CancelPendingRequests();
                client.Dispose();
                Log("Client Connection Closed");
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static User ChangePassword(ref User user, string email, string oldPassword, string newPassword, string webAPIKey)
        {
            var client = new HttpClient();

            var payload = new Dictionary<string, string>
                {
                    { "idToken" , user.idToken},
                    { "password" , newPassword },
                    { "returnSecureToken" , "true" }
                };

            var content = new FormUrlEncodedContent(payload);
            var response = client.PostAsync(changePasswordEndPoint + webAPIKey, content);
            response.Wait();
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();

            if (responseString.Result != null && responseString.Result != "")
            {
                var resetPasswordResponse = JsonConvert.DeserializeObject<ResetPassword>(responseString.Result);
                user.refreshToken = resetPasswordResponse.refreshToken;
                user.idToken = resetPasswordResponse.idToken;
                return user;
            }

            throw new Exception("Password Reset Failed. Try again");

        }


        /// <summary>
        /// Change user password using OOBCode
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static string ChangePasswordWithOOBCode(string code, string newPassword, string webAPIKey)
        {
            var client = new HttpClient();

            var payload = new Dictionary<string, string>
                {
                    { "oobCode" , code},
                    { "newPassword" , newPassword }
                };

            var content = new FormUrlEncodedContent(payload);
            var response = client.PostAsync(verifyResetPasswordEndPoint + webAPIKey, content);
            response.Wait();
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();

            if (responseString.Result != null && responseString.Result != "")
            {
                if (responseString.Result.Contains("NOT_ALLOWED") || responseString.Result.Contains("INVALID") || responseString.Result.Contains("EXPIRED") || responseString.Result.Contains("DISABLED"))
                    return "expired";

                var resetPasswordResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString.Result);
                Console.WriteLine(resetPasswordResponse["email"].ToString());
                return resetPasswordResponse["email"].ToString();
            }

            throw new Exception("Password Reset Failed. Try again");

        }


        /// <summary>
        /// Send user a reset password link
        /// </summary>
        /// <param name="email"></param>
        /// <param name="webAPIKey"></param>
        /// <returns></returns>
        public static bool ResetPassword(string email, string webAPIKey)
        {
            var client = new HttpClient();

            var payload = new Dictionary<string, string>
                {
                    { "requestType" , "PASSWORD_RESET"},
                    { "email" , email }
                };

            var content = new FormUrlEncodedContent(payload);
            var response = client.PostAsync(resetPasswordEndPoint + webAPIKey, content);
            response.Wait();
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();

            if (responseString.Result != null && responseString.Result != "")
            {
                if (responseString.Result.Contains("error"))
                    return false;

                var resetPasswordResponse = JsonConvert.DeserializeObject<ResetPassword>(responseString.Result);
                Console.WriteLine(resetPasswordResponse);
                return true;
            }

            throw new Exception("Password Reset Failed. Try again");

        }


        /// <summary>
        /// Verify the password reset code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="webAPIKey"></param>
        /// <returns></returns>
        public static string VerifyPasswordResetCode(string code, string webAPIKey)
        {
            var client = new HttpClient();

            var payload = new Dictionary<string, string>
                {
                    { "oobCode" , code}
                };

            var content = new FormUrlEncodedContent(payload);
            var response = client.PostAsync(verifyResetPasswordEndPoint + webAPIKey, content);
            response.Wait();
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();

            if (responseString.Result != null && responseString.Result != "")
            {
                if (responseString.Result.Contains("NOT_ALLOWED") || responseString.Result.Contains("INVALID") || responseString.Result.Contains("EXPIRED"))
                    return "expired";

                var resetPasswordResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString.Result);
                Console.WriteLine(resetPasswordResponse["email"].ToString());
                return resetPasswordResponse["email"].ToString();
            }

            throw new Exception("Password Reset Failed. Try again");
        }

        /// <summary>
        /// Sign Up New User Account
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="webAPIKey"></param>
        /// <returns></returns>
        public static User SignUp(string name, string email, string password, string webAPIKey)
        {
            if (name == null)
                return null;
            if (email == null)
                return null;
            if (password == null)
                return null;

            Log(string.Format("Begin Sign Up for {0}...", email));
            var client = new HttpClient();


            var taskResult = new TaskCompletionSource<string>();
            taskResult.SetResult("");

            try
            {
                var values = new Dictionary<string, string>
                {
                    {"displayName", name },
                    {"email", email },
                    {"password", password }
                };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(signUpEndPoint + webAPIKey, content);
                response.Wait();
                var responseString = response.Result.Content.ReadAsStringAsync();
                responseString.Wait();

                if (responseString.Result != null && responseString.Result != "")
                {
                    var userObject = JsonConvert.DeserializeObject<User>(responseString.Result);
                    var refreshToken = RefreshAuthToken(ref userObject, webAPIKey);
                    return userObject;
                }
                else
                    throw new UnauthorizedAccessException("Failed to sign up user");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }
            finally
            {
                client.CancelPendingRequests();
                client.Dispose();
                Log("Client Connection Closed");
            }
        }

        /// <summary>
        /// Sign Out Current User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="webAPIKey"></param>
        /// <returns></returns>
        public static User SignOut(User user, string webAPIKey)
        {
            if (user.localId != null)
            {
                Log(string.Format("Begin Sign Out for {0}...", user.email));
                var client = new HttpClient();

                try
                {
                    var values = new Dictionary<string, string>
                {
                    {"instanceId", user.instanceId },
                    {"localId", user.localId }
                };

                    var content = new FormUrlEncodedContent(values);
                    var response = client.PostAsync(signOutEndPoint + webAPIKey, content);
                    response.Wait();
                    var responseString = response.Result.Content.ReadAsStringAsync();
                    responseString.Wait();

                    if (responseString.Result != null && responseString.Result != "")
                    {
                        var userObject = JsonConvert.DeserializeObject<User>(responseString.Result);
                        return userObject;
                    }
                    else
                        throw new UnauthorizedAccessException("Failed to obtain Token. Check Credentials");
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    return null;
                }
                finally
                {
                    client.CancelPendingRequests();
                    client.Dispose();
                    Log("Client Connection Closed");
                }
            }
            return null;
        }

        /// <summary>
        /// Refresh Auth Token and retrieve userID
        /// </summary>
        /// <param name="user"></param>
        /// <param name="webAPIKey"></param>
        /// <returns></returns>
        public static RefreshToken RefreshAuthToken(ref User user, string webAPIKey)
        {
            var client = new HttpClient();

            //Get User ID
            var refreshValues = new Dictionary<string, string>
            {
                {"grant_type", "refresh_token" },
                {"refresh_token", user.refreshToken }
            };

            var content = new FormUrlEncodedContent(refreshValues);
            var response = client.PostAsync(authRefreshTokenEndPoint + webAPIKey, content);
            response.Wait();
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();

            if (responseString.Result != null && responseString.Result != "")
            {
                var tokenResponse = JsonConvert.DeserializeObject<RefreshToken>(responseString.Result);
                user.refreshToken = tokenResponse.refresh_token;
                user.userID = tokenResponse.user_id;
                user.idToken = tokenResponse.id_token;
                return tokenResponse; ;
            }

            return null;
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
