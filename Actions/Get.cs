using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharpFireStarter.Activity
{
    public class Get
    {
        public static Task<string> GetFromDB(string appID, string node, string oAuth)
        {
            //string endpoint = string.Format("{0}/{1}.json?access_token={2}", appID, node, oAuth);
            string endpoint = string.Format("{0}/{1}.json", appID, node);


            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            //httpWebRequest.ContentType = "application/json-patch+json; charset=utf-8";
            httpWebRequest.Method = "GET";
            //httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuth);

            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = reader.ReadToEnd();
                            Logger.Log(result);
                            return Task.FromResult(result);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            Logger.Log(error);
                        }
                    }

                }
            }
            return Task.FromResult(string.Empty);
        }
    }
}
