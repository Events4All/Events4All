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
    }
}