using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Events4All.Web.CustomAnnotations;


namespace Events4All.Web.Models
{
    public class ParticipantsViewModel
    {
        public int id { get; set; }
        public string EventName { get; set; }
        [Display(Name = "Event Start: ")]
        public DateTime? EventStartDate { get; set; }

        [ConfigRange()]
        public int NumberOfTicket { get; set; }

        [Display(Name = "Reminder:   ")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime? Reminder { get; set; }

        public double TicketPrice { get; set; }
        public double Subtotal { get; set; }

        [Display(Name = "Email: ")]
        public bool emailNotificationOn { get; set; }

        [Display(Name = "Text: ")]
        public bool SMSNotificationOn { get; set; }

        public string Description { get; set; }
        public List<Guid> Barcodes { get; set; }
    }


}