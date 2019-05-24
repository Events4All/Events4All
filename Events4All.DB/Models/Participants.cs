using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events4All.DB.Models
{
    public class Participants : Base
    {
        public ApplicationUser AccountID { get; set; }
        public Events EventID { get; set; }       
        public int NumberOfTicket { get; set; }  
        public DateTime? Reminder { get; set; }
    }
}
