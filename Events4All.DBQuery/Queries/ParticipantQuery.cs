﻿using Events4All.DB.Models;
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

        public ParticipantDTO MapParticipantToDTO(Participants participant)
        {
            ParticipantDTO dto = new ParticipantDTO();
            dto.eventId = participant.EventID.Id;
            dto.NumberOfTicket = participant.NumberOfTicket;
            dto.Reminder = participant.Reminder;
            dto.userId = participant.AccountID.Id;
            dto.emailNotificationOn = participant.emailNotificationOn;
            dto.SMSNotificationOn = participant.SMSNotificationOn;

            return dto;
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
