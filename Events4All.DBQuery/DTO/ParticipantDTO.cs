using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DBQuery
{
    public class ParticipantDTO
    {
        public int Id { get; set; }
        public int eventId { get; set; }
        public int NumberOfTicket { get; set; }
        public DateTime? Reminder { get; set; }
        public string userId { get; set; }
        public bool emailNotificationOn { get; set; }
        public bool SMSNotificationOn { get; set; }
        public List<Guid> Barcodes { get; set; }
        //public DateTime? EventStart { get; set; }
    }
}
