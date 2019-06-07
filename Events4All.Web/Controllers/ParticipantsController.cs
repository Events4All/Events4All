using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events4All.Web.Models;
using Events4All.DBQuery;
using Events4All.Web.Controllers;
using Events4All.DB.Models;
using System.IO;


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

                int participantID = query.CreateParticipant(dto);

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
            ViewBag.Username = userDTO.Username.Replace("@","").Replace(".com","");
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
                
                pDTO.Reminder = remindersViewModel.Reminder;
                pDTO.emailNotificationOn = remindersViewModel.emailNotificationOn;
                pDTO.SMSNotificationOn = remindersViewModel.SMSNotificationOn;
                pDTO.Id = id;

                pq.UpdateParticipantReminders(pDTO);
                
                 return RedirectToAction("ReminderConfirmation", "Participants");
            }

            return View(remindersViewModel);
        }

        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult ReminderConfirmation()
        {
            //   return RedirectToAction("ReminderConfirmation", "Participants");
            return View();
        }
    }
}

