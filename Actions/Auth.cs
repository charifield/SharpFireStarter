using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpFireStarter.Activity
{
    public static class Auth
    {
        private static readonly string authEndPoint = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=";

        /// <summary>
        /// Get oAuth Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static Task<string> Authenticate(string email, string password, string webAPIKey)
        {
            var client = new HttpClient();

            try
            {
                var taskResult = new TaskCompletionSource<string>();
                taskResult.SetResult("");

                try
                {
                    var values = new Dictionary<string, string>
                {
                    { "email", email },
                    {"password", password }
                };

                    var content = new FormUrlEncodedContent(values);
                    var response = client.PostAsync(authEndPoint + webAPIKey, content);
                    response.Wait();
                    var responseString = response.Result.Content.ReadAsStringAsync();
                    responseString.Wait();

                    if (responseString.Result != null && responseString.Result != "")
                    {
                        var objects = JObject.Parse(responseString.Result);
                        taskResult.SetResult(objects["idToken"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return taskResult.Task;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return Task.FromResult("");
            }
            finally
            {
                client.CancelPendingRequests();
                client.Dispose();
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
