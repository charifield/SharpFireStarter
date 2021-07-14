using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpFireStarter
{
    class FireBaseStorage
    {
        //Stored Variables

        private string storageURL { get; set; }
        private string webAPIKey { get; set; }
        private string oAuthToken { get; set; }
        public User currentUser { get; set; }


        /// <summary>
        /// Initialize FirebaseConnector with AppID and WebAPI Key (Found on your Firebase Console Online)
        /// </summary>
        /// <param name="appID"></param>
        public FireBaseStorage(string appID, string storageURL, string webAPIKey)
        {
            if (!Uri.IsWellFormedUriString(appID, UriKind.RelativeOrAbsolute))
                throw new UriFormatException("The given AppID URL Structure is not valid");

            this.storageURL = storageURL;
            this.webAPIKey = webAPIKey;

            Logger.Log("Initialized with AppID " + appID);
        }

        //public string Upload(Stream inputFile, string fileName)
        //{
        //    if (inputFile == null || fileName == null)
        //        throw new Exception("Required input file information was not provided");

        //    Upload
        //}
    }
}
