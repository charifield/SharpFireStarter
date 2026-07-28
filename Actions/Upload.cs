using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SharpFireStarter.Activity
{
    public static class Upload
    {
        /// <summary>
        /// Upload a file to Firebase Cloud Storage
        /// </summary>
        /// <param name="storageBucket">The Firebase storage bucket (e.g., "apiverve.appspot.com")</param>
        /// <param name="storagePath">The path in storage (e.g., "vervekit/jsonbin/abc123.json")</param>
        /// <param name="content">The content to upload as a string</param>
        /// <param name="oAuthToken">The Firebase auth token</param>
        /// <param name="contentType">The MIME type of the content (default: application/json)</param>
        /// <returns>The download URL for the uploaded file</returns>
        public static string UploadToCloudStorage(string storageBucket, string storagePath, string content, string oAuthToken, string contentType = "application/json")
        {
            try
            {
                // Construct the Firebase Storage upload URL
                string uploadUrl = $"https://firebasestorage.googleapis.com/v0/b/{storageBucket}/o?name={Uri.EscapeDataString(storagePath)}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
                httpWebRequest.ContentType = contentType;
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuthToken);

                // Convert content to bytes
                byte[] contentBytes = Encoding.UTF8.GetBytes(content);
                httpWebRequest.ContentLength = contentBytes.Length;

                // Write content to request stream
                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(contentBytes, 0, contentBytes.Length);
                }

                // Get response
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = reader.ReadToEnd();
                            Logger.Log("Upload successful: " + result);

                            // Parse response to get download token
                            var uploadResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                            // Extract download token from response (allows unauthenticated reads)
                            string downloadToken = null;
                            if (uploadResponse != null && uploadResponse.ContainsKey("downloadTokens"))
                            {
                                downloadToken = uploadResponse["downloadTokens"]?.ToString();
                            }

                            // Construct the download URL with token for public access
                            string downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{storageBucket}/o/{Uri.EscapeDataString(storagePath)}?alt=media";
                            if (!string.IsNullOrEmpty(downloadToken))
                            {
                                downloadUrl += $"&token={downloadToken}";
                            }

                            Logger.Log("Download URL: " + downloadUrl);
                            return downloadUrl;
                        }
                    }
                }

                return null;
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
                            Logger.Log("Upload error: " + error);
                        }
                    }
                }
                throw;
            }
        }

        /// <summary>
        /// Delete a file from Firebase Cloud Storage
        /// </summary>
        /// <param name="storageBucket">The Firebase storage bucket</param>
        /// <param name="storagePath">The path in storage</param>
        /// <param name="oAuthToken">The Firebase auth token</param>
        /// <returns>True if successful</returns>
        public static bool DeleteFromCloudStorage(string storageBucket, string storagePath, string oAuthToken)
        {
            try
            {
                string deleteUrl = $"https://firebasestorage.googleapis.com/v0/b/{storageBucket}/o/{Uri.EscapeDataString(storagePath)}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(deleteUrl);
                httpWebRequest.Method = "DELETE";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuthToken);

                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode == HttpStatusCode.NoContent)
                    {
                        Logger.Log("Delete successful");
                        return true;
                    }
                }

                return false;
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
                            Logger.Log("Delete error: " + error);
                        }
                    }
                }
                return false;
            }
        }
    }
}
