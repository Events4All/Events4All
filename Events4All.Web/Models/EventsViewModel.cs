using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Events4All.Web.Models
{
    public class EventsViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        [Display(Name = "Register/View Tickets")]
        public bool isRegistered { get; set; }

        public int participantId { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name = "Event Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        public string Name { get; set; }

        public ICollection<EventCategories> Categories { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string State { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string Zip { get; set; }

        public string Web { get; set; }

        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [Display(Name = "Start Time")]
        [CurrentDate(ErrorMessage = "Start Date can not be before current date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime? TimeStart { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        [NotEqual(PropName = "TimeStart", ErrorMessage = "End Date can not be before current date")]
        public DateTime? TimeStop { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string Description { get; set; }
        public string Detail { get; set; }
        public byte?[] Logo { get; set; }
        public String Number { get; set; }
        public string Street { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ticket Price")]
        [Range(0, 1000)]
        public double TicketPrice { get; set; }

        [Display(Name = "Hashtag")]
        public string HashTag { get; set; }

        [Display(Name = "Attendee Capacity")]
        public int AttendeeCap { get; set; }

        public int RemainingTickets { get; set; }


        //Custom Attributes

        public class NotEqualAttribute : ValidationAttribute
        {
            public string PropName { get; set; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult result;
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(PropName);
                var laterDate = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                DateTime earlierDate = (DateTime)value;

                if (laterDate > earlierDate)

                {
                    result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                else
                    result = null;

                return result;
            }
        }

        public class CurrentDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime dateTime = Convert.ToDateTime(value);

                if (dateTime >= DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
