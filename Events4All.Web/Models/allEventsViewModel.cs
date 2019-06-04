using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events4All.Web.Models
{
    public class allEventsViewModel
    {
        public int id { get; set; }
        public IEnumerable<Events4All.Web.Models.EventsViewModel> EventsCreated { get; set; }
        public IEnumerable<Events4All.Web.Models.ParticipantsViewModel> EventsAttend { get; set; }
    }
}