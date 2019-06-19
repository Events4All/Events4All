using Events4All.DB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;



namespace Events4All.DBQuery
{
    public class ParticipantQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsRegistered(int? id)
        {
            bool isRegistered = false;
            string userId = HttpContext.Current.User.Identity.GetUserId();

            List<Participants> participants = db.Participants
                .Include(i => i.EventID)
                .Include(i => i.AccountID)
                .Where(x => x.AccountID.Id == userId && x.EventID.Id == id)
                .ToList();

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

            List<Barcodes> barcodes = new List<Barcodes>();
            foreach(Guid barcode in participantsDTO.Barcodes)
            {
                var barcodeRecord = new Barcodes
                {
                    Barcode = barcode,
                    CreatedBy = user,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                barcodes.Add(barcodeRecord);
            }
            

            var participant = new Participants
            {
                AccountID = user,
                NumberOfTicket = participantsDTO.NumberOfTicket,
                Reminder = participantsDTO.Reminder,
                EventID = events,
                CreatedBy = user,
                IsActive = true,
                CreatedDate = DateTime.Now,
                Barcodes = barcodes
            };

            db.Participants.Add(participant);
            db.SaveChanges();
            
            return participant.Id;
        }

        public ParticipantDTO FindParticipant(int id)
        {
            Participants participant = db.Participants
                .Include(i=>i.EventID)
                .Include(i=>i.AccountID)
                .SingleOrDefault(x => x.Id == id);

            ParticipantDTO dto = MapParticipantToDTO(participant);
            return dto;
        }

        public int FindParticipantByEventAndUser(int? id)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();

            Participants participant = db.Participants
                .Include(i => i.EventID)
                .Include(i => i.AccountID)
                .Where(x => x.AccountID.Id == userId && x.EventID.Id == id)
                .SingleOrDefault();

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
            dto.Barcodes = GetBarcodeGuidsByParticipant(participant.Id);

            return dto;
        }

        public List<ParticipantDTO> QueryUserEventsAttending()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            List<ParticipantDTO> dtoList = new List<ParticipantDTO>();

            List<Participants> participantsList = db.Participants
                .Include(i => i.EventID)
                .Include(i => i.AccountID)
                .Where(i => i.AccountID.Id == userId && i.EventID.IsActive==true && i.EventID.TimeStart >= DateTime.Now)
                //.OrderBy(i => i.EventID.TimeStart)
                .ToList();

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

        public List<Guid> GetBarcodeGuidsByParticipant(int participantId)
        {
            List<Guid> barcodes = new List<Guid>();

            Participants participant = db.Participants
                .Include(x=>x.Barcodes)
                .Where(x=>x.Id == participantId && x.IsActive == true)
                .SingleOrDefault();

            foreach (Barcodes barcode in participant.Barcodes)
            {
                barcodes.Add(barcode.Barcode);
            }

            return barcodes;
        }

        public ParticipantDTO FindParticipantByBarcode(string guid)
        {
            Barcodes barcode = db.Barcodes
                .Where(x => x.Barcode.ToString() == guid && x.IsActive == true)
                .SingleOrDefault();

            ParticipantDTO pDTO = new ParticipantDTO();

            Participants participant = db.Participants
                .Include(x => x.Barcodes)
                .Include(x=>x.EventID)
                .Include(x=>x.AccountID)
                .Where(x => x.Barcodes.Select(y=>y.Id).ToList().Contains(barcode.Id) && x.IsActive == true)
                .SingleOrDefault();

            pDTO = MapParticipantToDTO(participant);
            return pDTO;
        }
    }
}
