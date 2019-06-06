using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;



namespace Events4All.DBQuery
{
    public class ParticipantQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsRegistered(int? id)
        {
            bool isRegistered = false;
            string userId = HttpContext.Current.User.Identity.GetUserId();
            List<Participants> participants = db.Participants.Include(i => i.EventID).Include(i => i.AccountID).Where(x => x.AccountID.Id == userId).Where(y => y.EventID.Id == id).ToList();

            if(participants.Count > 0)
            {
                isRegistered = true;
            }

            return isRegistered;
        }

        public int CreateParticipant(ParticipantDTO participantsDTO)
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
            return participants.Id;
        }

        public ParticipantDTO FindParticipant(int id)

        {
            Participants participant = db.Participants.Include(i=>i.EventID).Include(i=>i.AccountID).SingleOrDefault(x => x.Id == id);
            ParticipantDTO dto = MapParticipantToDTO(participant);
            return dto;
        }

        public int FindParticipantByEventAndUser(int? id)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            Participants participant = db.Participants.Include(i => i.EventID).Include(i => i.AccountID).Where(x => x.AccountID.Id == userId).SingleOrDefault(y => y.EventID.Id == id);
            int participantID = participant.Id;
            return participantID;
        }

        public ParticipantDTO MapParticipantToDTO(Participants participant)
        {
            ParticipantDTO dto = new ParticipantDTO();
            dto.Id = participant.Id;
            dto.eventId = participant.EventID.Id;
            dto.NumberOfTicket = participant.NumberOfTicket;
            dto.Reminder = participant.Reminder;
            dto.userId = participant.AccountID.Id;
            dto.emailNotificationOn = participant.emailNotificationOn;
            dto.SMSNotificationOn = participant.SMSNotificationOn;

            return dto;
        }

        public List<ParticipantDTO> QueryUserEventsAttending()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            List<ParticipantDTO> dtoList = new List<ParticipantDTO>();
            List<Participants> participantsList = db.Participants.Include(i => i.EventID).Include(i => i.AccountID).Where(i => i.AccountID.Id == userId).ToList();

            foreach (Participants userEvents in participantsList)
            {
                ParticipantDTO dto = MapParticipantToDTO(userEvents);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        
       public void UpdateParticipantReminders(ParticipantDTO pDTO)
        {            
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            Participants pRec = db.Participants.Find(pDTO.Id);
            pRec.Reminder = pDTO.Reminder;
            pRec.emailNotificationOn = pDTO.emailNotificationOn;
            pRec.SMSNotificationOn = pDTO.SMSNotificationOn;

            db.Entry(pRec).State = EntityState.Modified;
            db.SaveChanges();                     
        }
 
      
    }
}
