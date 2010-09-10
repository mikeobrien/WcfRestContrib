using System;
using System.IdentityModel.Selectors;

namespace NielsBohrLibrary.Runtime
{
    public class SecurityValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName != "tony" || password != "clifton")
                throw new Exception();
        }
    }
}
