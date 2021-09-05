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
        public static string GetFromDB(string databaseURL, string node, string oAuth, bool runAuthenticated, string orderBy, string startAt, string endAt, string equalTo, int limitToFirst, int limitToLast, bool shallow)
        {
            string url = string.Format("{0}/{1}.json", databaseURL, node);

            if (runAuthenticated)
                url += "?auth=" + oAuth;

            if (orderBy != null)
                if (url.Contains("?")) url += $"&orderBy=\"{orderBy}\""; else url += $"?orderBy=\"{orderBy}\"";

            if (startAt != null)
                if (url.Contains("?")) url += $"&startAt=\"{startAt}\""; else url += $"?startAt=\"{startAt}\"";

            if (endAt != null)
                if (url.Contains("?")) url += $"&endAt=\"{endAt}\""; else url += $"?endAt=\"{endAt}\"";

            if (equalTo != null)
                if (url.Contains("?")) url += $"&equalTo=\"{equalTo}\""; else url += $"?equalTo=\"{equalTo}\"";

            if (limitToFirst > 0)
                if (url.Contains("?")) url += $"&limitToFirst={limitToFirst}"; else url += $"?limitToFirst={limitToFirst}";

            if (limitToLast > 0)
                if (url.Contains("?")) url += $"&limitToLast={limitToLast}"; else url += $"?limitToLast={limitToLast}";

            if (shallow != false)
                if (url.Contains("?")) url += $"&shallow=true"; else url += $"?shallow=true";

            //if (oAuth == null || oAuth == "")
            //    url = url.Split('?')[0];

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
