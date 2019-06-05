﻿using Events4All.DB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Events4All.Web.Models
{
    public class ParticipantsViewModel
    {
        public int id { get; set; }
        public string EventName { get; set; }
        [Display(Name ="Event Start: ")]
        public DateTime? EventStartDate { get; set; }

        [Range(1, 3, ErrorMessage = "You must select between 1 and 3 tickets.")]
        public int NumberOfTicket { get; set; }
        [Display(Name ="Reminder:   ")]
        public DateTime? Reminder { get; set; }
        public double TicketPrice { get; set; }
        public double Subtotal { get; set; }

        [Display(Name ="Email: ")]
        public bool emailNotificationOn { get; set; }

        [Display(Name ="Text: ")]
        public bool SMSNotificationOn { get; set; }

        public string Description { get; set; }
    }


}