using Events4All.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Events4All.Web.CustomDataAnnotations
{
    public class PhoneReminderValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string pattern = "^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$";

            if ((string)value != ConstantValues.phoneValidation && value != null)
            {
                if (Regex.Match((string)value, pattern).Length > 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Please enter a valid phone number");
                }
            }
            else if ((string)value == ConstantValues.phoneValidation)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Phone Number is a required field");
            }
        }
    }
}
