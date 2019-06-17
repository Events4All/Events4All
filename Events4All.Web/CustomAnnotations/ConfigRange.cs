using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Events4All.Web.CustomAnnotations
{
    public class ConfigRangeAttribute : RangeAttribute, IClientValidatable
    {
        public ConfigRangeAttribute() :
            base
            (Convert.ToInt32(WebConfigurationManager.AppSettings["MinTickets"]),
             Convert.ToInt32(WebConfigurationManager.AppSettings["MaxTickets"]))
        { }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name, this.Minimum, this.Maximum);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(this.ErrorMessage),
                ValidationType = "range",
            };
            rule.ValidationParameters.Add("min", this.Minimum);
            rule.ValidationParameters.Add("max", this.Maximum);
            yield return rule;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            if (String.IsNullOrEmpty(value.ToString()))
                return null;
            else
            {
                var val = Convert.ToInt32(value);
                if (val >= Convert.ToInt32((WebConfigurationManager.AppSettings["MinTickets"])) && val <= Convert.ToInt32((WebConfigurationManager.AppSettings["MaxTickets"])))
                    return null;
            }

            return new ValidationResult(
                FormatErrorMessage(this.ErrorMessage)
            );
        }
    }
}