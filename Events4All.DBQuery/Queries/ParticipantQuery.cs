using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Events4All.DBQuery
{
    public class ParticipantQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void CreateParticipant(ParticipantDTO participantsDTO)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            Events events = db.Events.Find(participantsDTO.eventId);

            var participants = new Participants
            {
                AccountID = user,
                NumberOfTicket = participantsDTO.NumberOfTicket,
                Reminder = participantsDTO.Reminder,
                EventID = events,
                CreatedBy = user,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            db.Participants.Add(participants);
            db.SaveChanges();
        }

    }
}
