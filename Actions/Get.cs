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
        public static string GetFromDB(string appID, string node, string oAuth)
        {
            string url = string.Format("{0}/{1}.json?auth={2}", appID, node, oAuth);
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
