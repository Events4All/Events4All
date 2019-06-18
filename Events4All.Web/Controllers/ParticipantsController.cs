using Events4All.Business.ENotifications;
using Events4All.DBQuery;
using Events4All.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;


namespace Events4All.Web.Controllers
{
    public class ParticipantsController : Controller
    {
        [HttpGet]
        public ActionResult Create(int id)
        {         
            ParticipantsViewModel vm = new ParticipantsViewModel();
            EventQuery query = new EventQuery();
            EventDTO dto = new EventDTO();

            dto = query.FindEvent(id);
            vm.TicketPrice = dto.TicketPrice;

            return View(vm);
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumberOfTicket, Reminder")] ParticipantsViewModel participantsViewModel, int id)
        {
            ParticipantDTO dto = new ParticipantDTO();
            ParticipantQuery query = new ParticipantQuery();

            if (ModelState.IsValid)
            {
                dto.NumberOfTicket = participantsViewModel.NumberOfTicket;
                dto.Reminder = participantsViewModel.Reminder;
                dto.eventId = id;
                dto.Barcodes = new List<Guid>();

                for(int i = 0; i < dto.NumberOfTicket; i++)
                {
                    Guid barcode = Guid.NewGuid();
                    dto.Barcodes.Add(barcode);
                }

                int participantID = query.CreateParticipant(dto);

                EventDTO eventDTO = new EventDTO();
                ParticipantsViewModel vm = new ParticipantsViewModel();
                EventQuery eventQuery = new EventQuery();
                UserDTO userDTO = new UserDTO();
                UserQuery userQuery = new UserQuery();
                userDTO = userQuery.FindCurrentUser();
                eventDTO = eventQuery.FindEvent(dto.eventId);

                string content = System.IO.File.ReadAllText(Server.MapPath("~/ConfirmMail.cshtml"));
                content = content.Replace("{{Name}}", eventDTO.Name);
                content = content.Replace("{{Address}}", eventDTO.Address);
                content = content.Replace("{{City}}", eventDTO.City);
                content = content.Replace("{{State}}", eventDTO.State);
                content = content.Replace("{{Zip}}", eventDTO.Zip.ToString());
                content = content.Replace("{{TimeStart}}", eventDTO.TimeStart.ToString());
                content = content.Replace("{{Ticket}}", dto.NumberOfTicket.ToString());
                content = content.Replace("{{participantID}}", participantID.ToString());
                content = content.Replace("{{eventID}}", eventDTO.Id.ToString());
                var toEmail = userDTO.Username.ToString();
                new EmailNotification().SendEmail(toEmail, content, "Confirmation : You have registered for an event!");

                return RedirectToAction("RegistrationConfirmation/" + participantID, "Participants");
            }

            return View(participantsViewModel);
        }

        [HttpGet]
        public ActionResult RegistrationConfirmation(int id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            ParticipantDTO participantDTO = new ParticipantDTO();
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDTO = new EventDTO();

            ParticipantsViewModel vm = new ParticipantsViewModel();

            participantDTO = participantQuery.FindParticipant(id);
            eventDTO = eventQuery.FindEvent(participantDTO.eventId);

            vm.EventName = eventDTO.Name;
            vm.NumberOfTicket = participantDTO.NumberOfTicket;
            vm.EventStartDate = eventDTO.TimeStart;
            vm.Barcodes = participantDTO.Barcodes;

            ViewBag.EventId = eventDTO.Id;

            return View(vm);
        }

        public ActionResult RegistrationConfirmationPrint(int id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            ParticipantDTO participantDTO = new ParticipantDTO();
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDTO = new EventDTO();
            UserDTO userDTO = new UserDTO();
            UserQuery userQuery = new UserQuery();

            ParticipantsViewModel vm = new ParticipantsViewModel();

            participantDTO = participantQuery.FindParticipant(id);
            eventDTO = eventQuery.FindEvent(participantDTO.eventId);
            userDTO = userQuery.FindCurrentUser();

            vm.EventName = eventDTO.Name;
            vm.NumberOfTicket = participantDTO.NumberOfTicket;
            vm.EventStartDate = eventDTO.TimeStart;

            ViewBag.NumberOfTickets = participantDTO.NumberOfTicket;
            ViewBag.Username = userDTO.Username.Replace("@", "").Replace(".com", "");
            ViewBag.UserId = userDTO.Id;
            ViewBag.EventName = eventDTO.Name;
            ViewBag.EventId = eventDTO.Id;

            return View(vm);
        }

        [HttpGet]
        public ActionResult Reminders(int id)
        {
            RemindersViewModel rvm = new RemindersViewModel();
            ParticipantQuery pq = new ParticipantQuery();
            EventQuery eq = new EventQuery();

            ParticipantDTO pDTO = pq.FindParticipant(id);
            EventDTO evDTO = eq.FindEvent(pDTO.eventId);

            //Map DTO fields to rvm       

            rvm.EventStartDate = evDTO.TimeStart;
            rvm.Reminder = pDTO.Reminder;
            rvm.emailNotificationOn = pDTO.emailNotificationOn;
            rvm.SMSNotificationOn = pDTO.SMSNotificationOn;

            return View(rvm);
        }

        //POST REMINDER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reminders(int id, [Bind(Include ="Reminder, emailNotificationOn, SMSNotificationOn, TimeStart")] RemindersViewModel remindersViewModel)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(ModelState.IsValid)
            {
                ParticipantQuery pq = new ParticipantQuery();
                ParticipantDTO pDTO = new ParticipantDTO();
                ParticipantDTO pdt = pq.FindParticipant(id);
                ParticipantsViewModel pvm = new ParticipantsViewModel();
                
                EventQuery eq = new EventQuery();
                EventDTO edto = eq.FindEvent(pdt.eventId);

                pDTO.Reminder = remindersViewModel.Reminder;
                pDTO.emailNotificationOn = remindersViewModel.emailNotificationOn;
                pDTO.SMSNotificationOn = remindersViewModel.SMSNotificationOn;
                pDTO.Id = id;

                if (remindersViewModel.Reminder <= edto.TimeStart.Value)
                {
                    pq.UpdateParticipantReminders(pDTO);

                    return RedirectToAction("ReminderConfirmation", "Participants");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Reminder date is after the event.");
                }
            }
        
            return View(remindersViewModel);
        }


        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult ReminderConfirmation()
        {
            return View();
        }

        //public ActionResult SetReminder(int eventID)
        //{
        //    ParticipantQuery pq = new ParticipantQuery();
            
        //    var ParticipantID = pq.GetParticipantID(eventID);
            
        //    return RedirectToAction("Reminders/"+ ParticipantID,"Participants");
        //}
    }
}

