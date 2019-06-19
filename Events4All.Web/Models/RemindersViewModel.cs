using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Events4All.Web.Models;
using Events4All.Web.CustomDataAnnotations;

namespace Events4All.Web.Models
{
    public class RemindersViewModel
    {
        [Display(Name = "Event Start: ")]
        public DateTime? EventStartDate { get; set; }

        [Display(Name = "Reminder:   ")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        [NotEqual(PropName = "EventStartDate", ErrorMessage = "Please Enter A Date Between Now and the Event Start Date")]
        public DateTime? Reminder { get; set; }

        [Display(Name = "Email: ")]
        public bool emailNotificationOn { get; set; }

        [Display(Name = "Text: ")]
        public bool SMSNotificationOn { get; set; }

        [Display(Name = "Phone Number")]
        [PhoneReminderValidation]
        public string PhoneNumber { get; set; }

        public string PhoneValidation { get; set; }
        //public virtual EventsViewModel EventID { get; set; }
        //public IEnumerable<EventsViewModel> EventID { get; set; }

        //Custom Attributes
        public class NotEqualAttribute : ValidationAttribute
        {
            public string PropName { get; set; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
            ValidationResult result;
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(PropName);
                var eventDate= (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                DateTime ReminderDate = (DateTime)value;
                DateTime CurrentDate = DateTime.Now;

                if (ReminderDate >= eventDate || ReminderDate <= CurrentDate)

                {
                    result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                else
                {
                    result = null;
                    
                }
                return result;
            }
        }
        
    }
}
