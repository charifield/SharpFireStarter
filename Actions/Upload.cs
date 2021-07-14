using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SharpFireStarter.Activity
{
    public static class Upload
    {

        private static readonly string uploadEndPoint = "https://firebasestorage.googleapis.com/v0/b/";


        //public static string UploadFile(string endpoint, Stream inputFile, string fileName)
        //{
        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
        //    //httpWebRequest.ContentType = "application/json; charset=utf-8";
        //    httpWebRequest.ContentType = "application/json-patch+json; charset=utf-8";
        //    httpWebRequest.Method = "PATCH";
        //    //httpWebRequest.Headers.Add("Authorization", "Bearer " + oAuth);

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        //string json = "[  { \"ReferenceId\": \"a123\"  } ]";
        //        var newData = "";
        //        if (data is string)
        //            newData = data.ToString();
        //        else
        //            newData = JsonConvert.SerializeObject(data);
        //        streamWriter.Write(newData);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }
        //    try
        //    {
        //        using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
        //        {
        //            if (httpWebRequest.HaveResponse && response != null)
        //            {
        //                using (var reader = new StreamReader(response.GetResponseStream()))
        //                {
        //                    var result = reader.ReadToEnd();
        //                    Logger.Log(result);
        //                }
        //            }
        //        }
        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Response != null)
        //        {
        //            using (var errorResponse = (HttpWebResponse)e.Response)
        //            {
        //                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
        //                {
        //                    string error = reader.ReadToEnd();
        //                    Logger.Log(error);
        //                }
        //            }

        //        }
        //    }
        //}
    }
}
