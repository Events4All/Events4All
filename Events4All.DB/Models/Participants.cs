using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Events4All.DB.Models
{
    public class Participants : Base
    {
        public ApplicationUser AccountID { get; set; }       
        public ICollection<Barcodes> Barcodes { get; set; }
        public Events EventID { get; set; }       
        public int NumberOfTicket { get; set; }  
        public DateTime? Reminder { get; set; }
        
        [Display (Name ="Email: ")]
        public bool emailNotificationOn { get; set; }

        [Display (Name ="Text: ")]
        public bool SMSNotificationOn { get; set; } 
        
        public DateTime? EmailNotificationSentTime { get; set; }
    }
}
