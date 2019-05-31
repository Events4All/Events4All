using Events4All.DB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events4All.Web.Models
{
    public class ParticipantsViewModel
    {
        public int id { get; set; }
        public string EventName { get; set; }
        public DateTime? EventStartDate { get; set; }

        [Range(1, 3, ErrorMessage = "You must select between 1 and 3 tickets.")]
        public int NumberOfTicket { get; set; }

        public DateTime? Reminder { get; set; }
        public double TicketPrice { get; set; }
        public double Subtotal { get; set; }
        public string Description { get; set; }
    }
}


