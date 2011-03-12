using System.IdentityModel.Selectors;
using System.ServiceModel;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.IdentityModel.Selectors
{
    public class UserNamePasswordValidatorProxy<T> : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            var validator = OperationContext.Current.Host.Description.GetObjectFactory().Create<UserNamePasswordValidator>(typeof(T));
            validator.Validate(userName, password);
        }
    }
}
