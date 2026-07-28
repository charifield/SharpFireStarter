using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SharpFireStarter.Activity
{
    public static class Set
    {

        public static void WriteToDB(string appID, string node, string oAuth, object data, bool runAuthenticated, bool silent)
        {
            string endpoint = "";

            if (runAuthenticated && (oAuth != null && oAuth != ""))
            {
                endpoint = string.Format("{0}/{1}.json?auth={2}", appID, node, oAuth);
            }
            else
            {
                endpoint = string.Format("{0}/{1}.json", appID, node);
            }

            if (silent)
            {
                if (endpoint.Contains("?"))
                    endpoint += $"&print=silent";
                else
                    endpoint += $"?print=silent";
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            //httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.ContentType = "application/json-patch+json; charset=utf-8";
            httpWebRequest.Method = "PATCH";
            //httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuth);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var newData = "";
                if (data is string)
                {
                    newData = data.ToString();
                }
                else if (data is bool || data is int || data is double || data is float)
                {
                    // For primitive types, create a JSON object wrapper
                    newData = JsonConvert.SerializeObject(new { value = data });
                }
                else
                {
                    newData = JsonConvert.SerializeObject(data);
                }
                streamWriter.Write(newData);
                streamWriter.Flush();
                streamWriter.Close();
            }
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
                        }
                    }
                }
            }
            catch (WebException e)
            {
                string error = "Unknown error";
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                            Logger.Log(error);
                        }
                    }
                }
                // CRITICAL FIX: Re-throw the exception so callers know the write failed
                // Previously this was swallowed silently, causing data loss
                throw new Exception($"Firebase write failed: {error}", e);
            }
        }

        /// <summary>
        /// Set (replace) a node in Firebase DB using PUT instead of PATCH
        /// Use this when you want to completely replace a node rather than merge
        /// </summary>
        public static void SetToDB(string appID, string node, string oAuth, object data, bool runAuthenticated, bool silent)
        {
            string endpoint = "";

            if (runAuthenticated && (oAuth != null && oAuth != ""))
            {
                endpoint = string.Format("{0}/{1}.json?auth={2}", appID, node, oAuth);
            }
            else
            {
                endpoint = string.Format("{0}/{1}.json", appID, node);
            }

            if (silent)
            {
                if (endpoint.Contains("?"))
                    endpoint += $"&print=silent";
                else
                    endpoint += $"?print=silent";
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "PUT";  // PUT replaces entirely, PATCH merges

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var newData = "";
                if (data == null)
                {
                    newData = "null";
                }
                else if (data is string)
                {
                    newData = data.ToString();
                }
                else if (data is bool || data is int || data is double || data is float)
                {
                    newData = JsonConvert.SerializeObject(data);
                }
                else
                {
                    newData = JsonConvert.SerializeObject(data);
                }
                streamWriter.Write(newData);
                streamWriter.Flush();
                streamWriter.Close();
            }
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
                        }
                    }
                }
            }
            catch (WebException e)
            {
                string error = "Unknown error";
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                            Logger.Log(error);
                        }
                    }
                }
                throw new Exception($"Firebase set failed: {error}", e);
            }
        }
    }
}
