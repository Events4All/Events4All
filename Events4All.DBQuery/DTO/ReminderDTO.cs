using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DBQuery.DTO
{
    public class ReminderDTO
    {
        public string ParticipantId { get; set; }
        public string EventId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TimeStartShort { get; set; }
        public string TimeStopShort { get; set; }
        public string TimeStart12Hr { get; set; }
        public string TimeStop12Hr { get; set; }
        public string Subject { get; set; }
        public string Phone { get; set; }
        public bool SMSOn { get; set; }
        public bool EmailOn { get; set; }
    }
}
