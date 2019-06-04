using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DBQuery
{
    public class EventDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public ICollection<EventCategories> Categories { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Web { get; set; }
        public string TwitterHandle { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeStop { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public byte?[] Logo { get; set; }
        public double TicketPrice { get; set; }
        public string HashTag { get; set; }
        public DateTime? Reminder { get; set; }

        public bool emailNotificationOn { get; set; }
        public bool SMSNotificationOn { get; set; }

    }
}
