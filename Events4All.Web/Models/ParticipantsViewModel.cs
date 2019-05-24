using Events4All.DB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events4All.Web.Models
{
    public class ParticipantsViewModel
    {
        public int NumberOfTicket { get; set; }
        public DateTime? Reminder { get; set; }
        public double TicketPrice { get; set; }
        public double Subtotal { get; set; }
    }
}