using System.IdentityModel.Selectors;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.IdentityModel.Selectors
{
    public class UserNamePasswordValidatorProxy<T> : UserNamePasswordValidator where T : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            DependencyResolver.Current.Create<UserNamePasswordValidator>(typeof(T)).Validate(userName, password);
        }
    }
}
