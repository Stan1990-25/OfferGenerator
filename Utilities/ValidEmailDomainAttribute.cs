using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string mAllowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain) : base(allowedDomain)
        {
            mAllowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            string valToString = value.ToString().ToLower();
            return valToString.EndsWith(mAllowedDomain.ToLower());
        }
    }
}
