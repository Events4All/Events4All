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

namespace Events4All.Web.Controllers
{
    public class ParticipantsController : Controller
    {
        private ParticipantQuery query = new ParticipantQuery();
        private ParticipantDTO dto = new ParticipantDTO();

        private EventQuery eventQuery = new EventQuery();
        private EventDTO eventDTO = new EventDTO();

        [HttpGet]
        public ActionResult Create(int id)
        {         
            ParticipantsViewModel vm = new ParticipantsViewModel();

            eventDTO = eventQuery.FindEvent(id);
            vm.TicketPrice = eventDTO.TicketPrice;

            return View(vm);
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumberOfTicket, Reminder")] ParticipantsViewModel participantsViewModel, int id)
        {
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
            ParticipantsViewModel vm = new ParticipantsViewModel();
            dto = query.FindParticipant(id);
            eventDTO = eventQuery.FindEvent(dto.eventId);

            vm.EventName = eventDTO.Name;
            vm.NumberOfTicket = dto.NumberOfTicket;
            vm.EventStartDate = eventDTO.TimeStart;

            return View(vm);
        }
        // public ParticipantsController() { }
        //GET REMINDER
        //public Participants pt = new Participants();
        //public IQueryable<ParticipantDTO> GetParticipants()
        //{ 
        //    var participants = from p in 

        //}


        [HttpGet]
        public ActionResult Reminders(int id)
        {         
            ParticipantsViewModel pvm = new ParticipantsViewModel();

            ParticipantDTO pDTO = query.FindParticipant(id);
            EventDTO evDTO = eventQuery.FindEvent(pDTO.eventId);

            //Map DTO fields to pvm
            pvm.EventStartDate = evDTO.TimeStart;
            pvm.Reminder = pDTO.Reminder;
            //if (pvm.emailNotificationOn == true)
                
            pvm.emailNotificationOn = pDTO.emailNotificationOn;
            pvm.SMSNotificationOn = pDTO.SMSNotificationOn;
             
            return View(pvm);
        }


        //POST REMINDER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reminders(int id, [Bind(Include ="reminder, emailNotificationOn, SMSNotificationOn, TimeStart")] ParticipantsViewModel participantsViewModel)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 


            if(ModelState.IsValid)
            {
                ParticipantDTO pDTO = new ParticipantDTO();
                pDTO.Reminder = participantsViewModel.Reminder;
                pDTO.emailNotificationOn= participantsViewModel.emailNotificationOn;
                pDTO.SMSNotificationOn= participantsViewModel.SMSNotificationOn;
                pDTO.Id = id;

                query.UpdateParticipantReminders(pDTO);
                return RedirectToAction("Index", "Home");
            }

            return View(participantsViewModel);
        }




        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

