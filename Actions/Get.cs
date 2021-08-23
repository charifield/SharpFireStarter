﻿using System;
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
        public static string GetFromDB(string databaseURL, string node, string oAuth, bool runAuthenticated, string orderBy, int limitToFirst, int limitToLast)
        {
            string url = string.Format("{0}/{1}.json", databaseURL, node);

            if (runAuthenticated)
                url += "?auth=" + oAuth;

            if (orderBy != null)
                if (url.Contains("?")) url += $"&orderBy=\"{orderBy}\""; else url += $"?orderBy=\"{orderBy}\"";

            if(limitToFirst > 0)
                if (url.Contains("?")) url += $"&limitToFirst={limitToFirst}"; else url += $"?limitToFirst={limitToFirst}";

            if (limitToLast > 0)
                if (url.Contains("?")) url += $"&limitToLast={limitToLast}"; else url += $"?limitToLast={limitToLast}";

            if (oAuth == null || oAuth == "")
                url = url.Split('?')[0];
            var req = (HttpWebRequest)WebRequest.Create(url);
            //req.Credentials = new NetworkCredential(_bcApiUserName, _bcApiUserPassword);
            req.ContentType = "application/json";
            req.Accept = "application/json";

            req.Method = "GET";

            var response = req.GetResponse();
            string responseBody = "";
            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                responseBody = reader.ReadToEnd();
            }
            return responseBody;
        }
    }
}
