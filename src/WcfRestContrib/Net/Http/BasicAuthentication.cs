using System;
using System.Text;
using System.Net;
using System.IdentityModel.Selectors;

namespace WcfRestContrib.Net.Http
{
    public class BasicAuthentication 
    {
        private string _username;
        private string _password;
        private bool _valid;

        public BasicAuthentication(WebHeaderCollection headers)
        {
            GetCredentials(headers);
        }
        
        public string Username { get { return _username; } }

        public bool Authenticate(string username, string password)
        {
            return _valid && _username == username && _password == password;
        }

        public bool Authenticate(UserNamePasswordValidator validator)
        {
            if (!_valid) return false;
            try
            {
                validator.Validate(_username, _password);
                return true;
            }
            catch { }
            return false;
        }

        public static void SetUnauthorizedHeader(WebHeaderCollection headers, string realm)
        {
            headers["WWW-Authenticate"] = 
                string.Format("Basic realm=\"{0}\"", realm);
        }

        private void GetCredentials(WebHeaderCollection headers)
        {
            var credentials = headers["Authorization"];
            if (credentials != null) credentials = credentials.Trim();

            if (!string.IsNullOrEmpty(credentials))
            {
                try
                {
                    var credentialParts = credentials.Split(new [] { ' ' });
                    if (credentialParts.Length == 2 && 
                        credentialParts[0].Equals("basic", 
                        StringComparison.OrdinalIgnoreCase))
                    {
                        credentials = Encoding.ASCII.GetString(
                            Convert.FromBase64String(credentialParts[1]));
                        credentialParts = credentials.Split(new [] { ':' }, 2);
                        if (credentialParts.Length == 2)
                        {
                            _username = credentialParts[0];
                            _password = credentialParts[1];
                            _valid = true;
                            return;
                        }
                    }
                }
                catch { }
            }

           _valid = false;
        }
    }
}
