using System;
using System.ComponentModel.DataAnnotations;

namespace Events4All.Web.CustomAnnotations
{
    public class AtendeeCapValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(Convert.ToInt32(value) >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("There are not enough tickets remaining");
            }
            
        }
    }
}