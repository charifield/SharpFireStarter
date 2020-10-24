using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFireStarter.Models
{
    class ResetPassword
    {
        public string localId { get; set; }
        public string email { get; set; }
        public string idToken { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
    }
}
