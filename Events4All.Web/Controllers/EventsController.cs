﻿using Events4All.DBQuery;
using Events4All.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using Events4All.DBQuery.DTO;
using Events4All.DBQuery.Queries;
using Events4All.Business.Rules;

namespace Events4All.Web.Controllers
{
    public class EventsController : Controller
    {
        public ActionResult Details(int? id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            EventQuery Equery = new EventQuery();
            EventDTO Edto = new EventDTO();

            

            if (participantQuery.IsRegistered(id))
            {
                ViewBag.Registered = "You have already registered for this event.";
                ViewBag.ParticipantID = participantQuery.FindParticipantByEventAndUser(id);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edto = Equery.FindEvent(id);

            if (Edto == null)
            {
                return HttpNotFound();
            }

            int ticketsRemaining = Equery.GetRemainingTickets(Edto.Id);

            if(ticketsRemaining == 0)
            {
                ViewBag.SoldOut = "This event is sold out";
            }
            else if(ticketsRemaining < 0)
            {
                Exception ex = new Exception("There are a negative number of tickets remaining for this event. This should never execute. If so, there is a bug");
                throw ex;
            }

            EventsViewModel vm = new EventsViewModel();


            vm.Address = Edto.Address;
            vm.Categories = Edto.Categories;
            vm.City = Edto.City;
            vm.CreatedDate = Edto.CreatedDate;
            vm.Description = Edto.Description;
            vm.Detail = Edto.Detail;
            vm.HashTag = Edto.HashTag;
            vm.Logo = Edto.Logo;
            vm.Id = Edto.Id;
            vm.IsActive = Edto.IsActive;
            vm.Name = Edto.Name;
            vm.State = Edto.State;
            vm.TicketPrice = Edto.TicketPrice;
            vm.TimeStart = Edto.TimeStart;
            vm.TimeStop = Edto.TimeStop;
            vm.TwitterHandle = Edto.TwitterHandle;
            vm.Web = Edto.Web;
            vm.Zip = Edto.Zip;


            string fullAddressRaw = Edto.Address + " " + Edto.City + " " + Edto.State + " " + Edto.Zip + " ";
            string trimRawAddress = fullAddressRaw.Trim(); //Edto.Address.Trim();
            int spaceLoc = trimRawAddress.IndexOf(' ');
            string number = trimRawAddress.Substring(0, spaceLoc);
            string street = trimRawAddress.Substring(spaceLoc + 1);

            vm.Number = number;
            vm.Street = street;

            vm.AttendeeCap = Edto.AttendeeCap;

            return View(vm);
        }

        public ActionResult Index(int? id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            EventQuery Equery = new EventQuery();
            EventDTO Edto = new EventDTO();

            if (participantQuery.IsRegistered(id))
            {
                ViewBag.ParticipantID = participantQuery.FindParticipantByEventAndUser(id);
            }

            List<EventsViewModel> vmList = new List<EventsViewModel>();
            List<EventDTO> dtoEventList = Equery.QueryIndexData();

            foreach (EventDTO eventDto in dtoEventList)
            {
                if (eventDto.IsActive == true)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Address = eventDto.Address;
                    vm.Categories = eventDto.Categories;
                    vm.City = eventDto.City;
                    vm.CreatedDate = eventDto.CreatedDate;
                    vm.Description = eventDto.Description;
                    vm.Detail = eventDto.Detail;
                    vm.HashTag = eventDto.HashTag;
                    vm.Id = eventDto.Id;
                    vm.IsActive = eventDto.IsActive;
                    vm.Logo = eventDto.Logo;
                    vm.Name = eventDto.Name;
                    vm.State = eventDto.State;
                    vm.TicketPrice = eventDto.TicketPrice;
                    vm.TimeStart = eventDto.TimeStart;
                    vm.TimeStop = eventDto.TimeStop;
                    vm.TwitterHandle = eventDto.TwitterHandle;
                    vm.Web = eventDto.Web;
                    vm.Zip = eventDto.Zip;
                    vm.AttendeeCap = eventDto.AttendeeCap;

                    vm.isRegistered = participantQuery.IsRegistered(eventDto.Id);
                    if (vm.isRegistered)
                    {
                        vm.participantId = participantQuery.FindParticipantByEventAndUser(eventDto.Id);
                    }                  
                    vmList.Add(vm);
                }
            }

            return View(vmList);
        }

        [Authorize]
        // GET: Events/Create
        public ActionResult Create()
        {
            EventsViewModel eventsViewModel = new EventsViewModel();

            return View(eventsViewModel);
        }
        

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,City,State,Zip,Web,TwitterHandle,TimeStart,TimeStop,Description,Detail,TicketPrice,HashTag,AttendeeCap")] EventsViewModel eventsViewModel)
        {
            EventDTO Edto = new EventDTO();
            EventQuery Equery = new EventQuery();

            //ModelState.Remove("CreatedBy");
            if (ModelState.IsValid)
            {
                //db.Events.Add(events);
                //db.SaveChanges();
                Edto.Name = eventsViewModel.Name;
                Edto.Address = eventsViewModel.Address;
                Edto.City = eventsViewModel.City;
                Edto.State = eventsViewModel.State;
                Edto.Zip = eventsViewModel.Zip;
                Edto.Web = eventsViewModel.Web;
                Edto.TwitterHandle = eventsViewModel.TwitterHandle;
                Edto.TimeStart = eventsViewModel.TimeStart;
                Edto.TimeStop = eventsViewModel.TimeStop;
                Edto.Description = eventsViewModel.Description;
                Edto.Detail = eventsViewModel.Detail;
                Edto.TicketPrice = eventsViewModel.TicketPrice;
                Edto.HashTag = eventsViewModel.HashTag;
                Edto.AttendeeCap = eventsViewModel.AttendeeCap;

                Equery.CreateEvent(Edto);

                return RedirectToAction("Index");
            }

            return View(eventsViewModel);
        }

        //get
        public ActionResult Edit(int? id)
        {
            EventQuery Equery = new EventQuery();
            EventDTO Edto = new EventDTO();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edto = Equery.FindEvent(id);

            if (Edto == null)
            {
                return HttpNotFound();
            }

            EventsViewModel vm = new EventsViewModel();
            vm.Address = Edto.Address;
            vm.Categories = Edto.Categories;
            vm.City = Edto.City;
            vm.Zip = Edto.Zip;
            vm.Description = Edto.Description;
            vm.Detail = Edto.Detail;
            vm.HashTag = Edto.HashTag;
            vm.Id = Edto.Id;
            vm.IsActive = Edto.IsActive;
            vm.Logo = Edto.Logo;
            vm.Name = Edto.Name;
            vm.State = Edto.State;
            vm.TicketPrice = Edto.TicketPrice;
            vm.TimeStart = Edto.TimeStart;
            vm.TimeStop = Edto.TimeStop;
            vm.TwitterHandle = Edto.TwitterHandle;
            vm.Web = Edto.Web;
            vm.AttendeeCap = Edto.AttendeeCap;


            return View(vm);
        }

        //post
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventsViewModel EVM)
        {
            EventQuery Equery = new EventQuery();
            EventDTO Edto = new EventDTO();

            Edto.Id = EVM.Id;
            Edto.Name = EVM.Name;
            Edto.Address = EVM.Address;
            Edto.City = EVM.City;
            Edto.State = EVM.State;
            Edto.Zip = EVM.Zip;
            Edto.Detail = EVM.Detail;
            Edto.Categories = EVM.Categories;
            Edto.Description = EVM.Description;
            Edto.Logo = EVM.Logo;
            Edto.TicketPrice = EVM.TicketPrice;
            Edto.TimeStart = EVM.TimeStart;
            Edto.TimeStop = EVM.TimeStop;
            Edto.HashTag = EVM.HashTag;
            Edto.TwitterHandle = EVM.TwitterHandle;
            Edto.Web = EVM.Web;
            Edto.AttendeeCap = EVM.AttendeeCap;

            Equery.EditEvent(Edto);
            return RedirectToAction("Index");
        }

        // GET
        public ActionResult Delete(int? id)
        {
            EventQuery Equery = new EventQuery();
            EventDTO Edto = new EventDTO();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edto = Equery.FindEvent(id);
            if (Edto == null)
            {
                return HttpNotFound();
            }
            EventsViewModel vm = new EventsViewModel();
            vm.Address = Edto.Address;
            vm.Categories = Edto.Categories;
            vm.City = Edto.City;
            vm.Description = Edto.Description;
            vm.Detail = Edto.Detail;
            vm.Zip = Edto.Zip;
            vm.HashTag = Edto.HashTag;
            vm.Id = Edto.Id;
            vm.IsActive = Edto.IsActive;
            vm.Logo = Edto.Logo;
            vm.Name = Edto.Name;
            vm.State = Edto.State;
            vm.CreatedDate = Edto.CreatedDate;
            vm.TicketPrice = Edto.TicketPrice;
            vm.TimeStart = Edto.TimeStart;
            vm.TimeStop = Edto.TimeStop;
            vm.TwitterHandle = Edto.TwitterHandle;
            vm.Web = Edto.Web;
            vm.AttendeeCap = Edto.AttendeeCap;


            return View(vm);

        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            EventQuery Equery = new EventQuery();

            Equery.DeleteConfirmed(id);
            return RedirectToAction("Index");
        }


        public PartialViewResult SelectCalendarType(int id)
        {
            ViewBag.EventId = id;
            return PartialView();
        }

        public ActionResult SelectCalendarEmail(int id)
        {
            ViewBag.EventId = id;
            return View();
        }

        public void DownloadCalendar(int id)
        {
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDto = eventQuery.FindEvent(id);
            byte[] ics = GenerateICSFile(eventDto);
            Response.Clear();
            Response.ContentType = "text/calendar";
            Response.AddHeader("content-disposition", "attachment; filename=" + eventDto.Name + ".ics");
            Response.BinaryWrite(ics);
            Response.End();
        }

        public byte[] GenerateICSFile(EventDTO eventDto)
        {
            var calendar = new Calendar();

            calendar.Events.Add(new CalendarEvent
            {
                Class = "PUBLIC",
                Summary = eventDto.Name + " - " + eventDto.Description,
                Created = new CalDateTime(DateTime.Now),
                //Description = dto.Detail,
                Start = new CalDateTime(eventDto.TimeStart.Value),
                End = new CalDateTime(eventDto.TimeStop.Value),
                Sequence = 0,
                Uid = Guid.NewGuid().ToString(),
                Location = eventDto.Address + " " + eventDto.City + " " + eventDto.State + " " + eventDto.Zip,
            });

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);
            byte[] calendarBytes = System.Text.Encoding.UTF8.GetBytes(serializedCalendar);
            return calendarBytes;
        }

        public PartialViewResult _EventsCreatedPartial()
        {
            {
                EventQuery Equery = new EventQuery();
                ParticipantQuery Pquery = new ParticipantQuery();


                allEventsViewModel UserEventsCreatedList = new allEventsViewModel();
                List<EventsViewModel> events = new List<EventsViewModel>();

                List<EventDTO> dtoUserEventsCreated = Equery.QueryUserEventsCreated();

                foreach (EventDTO Edto in dtoUserEventsCreated)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Id = Edto.Id;
                    vm.Name = Edto.Name;
                    vm.TimeStart = Edto.TimeStart;
                    vm.Description = Edto.Description;

                    events.Add(vm);
                }

                List<ParticipantsViewModel> Pevents = new List<ParticipantsViewModel>();
                List<ParticipantDTO> ParticipantDTO = Pquery.QueryUserEventsAttending();

                foreach (ParticipantDTO Pdto in ParticipantDTO)
                {
                    ParticipantsViewModel vm = new ParticipantsViewModel();

                    vm.id = Equery.FindEvent(Pdto.eventId).Id;
                    vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                    vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                    vm.Description = Equery.FindEvent(Pdto.eventId).Description;

                    Pevents.Add(vm);
                }

                UserEventsCreatedList.EventsAttend = Pevents;
                UserEventsCreatedList.EventsCreated = events;

                return PartialView(UserEventsCreatedList);
            }
        }

        public PartialViewResult _eventsAttended(int? id)
        {
            {
                EventQuery Equery = new EventQuery();
                ParticipantQuery Pquery = new ParticipantQuery();

                allEventsViewModel UserEventsCreatedList = new allEventsViewModel();
                List<EventsViewModel> events = new List<EventsViewModel>();

                List<EventDTO> dtoUserEventsCreated = Equery.QueryUserEventsCreated();

                foreach (EventDTO Edto in dtoUserEventsCreated)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Id = Edto.Id;
                    vm.Name = Edto.Name;
                    vm.TimeStart = Edto.TimeStart;
                    vm.Description = Edto.Description;
                    events.Add(vm);
                }

                List<ParticipantsViewModel> Pevents = new List<ParticipantsViewModel>();
                List<ParticipantDTO> ParticipantDTO = Pquery.QueryUserEventsAttending();

                foreach (ParticipantDTO Pdto in ParticipantDTO)
                {
                    ParticipantsViewModel vm = new ParticipantsViewModel();

                    vm.id = Equery.FindEvent(Pdto.eventId).Id;
                    vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                    vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                    vm.Description = Equery.FindEvent(Pdto.eventId).Description;
                    Pevents.Add(vm);
                }

                UserEventsCreatedList.EventsAttend = Pevents;
                UserEventsCreatedList.EventsCreated = events;

                return PartialView(UserEventsCreatedList);
            }
        }

        [Authorize]
        public ActionResult allEvents(int? id)
        {
            {
                EventQuery Equery = new EventQuery();
                ParticipantQuery Pquery = new ParticipantQuery();
                
                allEventsViewModel UserEventsCreatedList = new allEventsViewModel();
                
                List<EventsViewModel> events = new List<EventsViewModel>();

                List<EventDTO> dtoUserEventsCreated = Equery.QueryUserEventsCreated();


                foreach (EventDTO Edto in dtoUserEventsCreated)
                {
                   
                        EventsViewModel vm = new EventsViewModel();

                        vm.Id = Edto.Id;
                        vm.Name = Edto.Name;
                        vm.TimeStart = Edto.TimeStart;
                        vm.Description = Edto.Description;

                        vm.isRegistered = Pquery.IsRegistered(Edto.Id);
                        if (vm.isRegistered)
                        {
                            vm.participantId = Pquery.FindParticipantByEventAndUser(Edto.Id);
                        }
                        events.Add(vm);
                    
                }

                List<ParticipantsViewModel> Pevents = new List<ParticipantsViewModel>();
                List<ParticipantDTO> ParticipantDTO = Pquery.QueryUserEventsAttending();

                foreach (ParticipantDTO Pdto in ParticipantDTO)
                {
                    ParticipantsViewModel vm = new ParticipantsViewModel();

                    vm.id = Equery.FindEvent(Pdto.eventId).Id;
                    vm.parId = Pdto.Id;
                    vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                    vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                    vm.Description = Equery.FindEvent(Pdto.eventId).Description;
                    Pevents.Add(vm);
                }

                UserEventsCreatedList.EventsAttend = Pevents;
                UserEventsCreatedList.EventsCreated = events;


                return View(UserEventsCreatedList);
            }
        }

        [Authorize]
        public ActionResult eventsAttended()
        {
            {
                EventQuery Equery = new EventQuery();
                ParticipantQuery Pquery = new ParticipantQuery();

                allEventsViewModel UserEventsCreatedList = new allEventsViewModel();
                List<EventsViewModel> events = new List<EventsViewModel>();

                List<EventDTO> dtoUserEventsCreated = Equery.QueryUserEventsCreated();


                foreach (EventDTO Edto in dtoUserEventsCreated)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Id = Edto.Id;
                    vm.Name = Edto.Name;
                    vm.TimeStart = Edto.TimeStart;
                    vm.Description = Edto.Description;
                    events.Add(vm);
                }

                List<ParticipantsViewModel> Pevents = new List<ParticipantsViewModel>();
                List<ParticipantDTO> ParticipantDTO = Pquery.QueryUserEventsAttending();

                foreach (ParticipantDTO Pdto in ParticipantDTO)
                {
                    ParticipantsViewModel vm = new ParticipantsViewModel();

                    vm.id = Equery.FindEvent(Pdto.eventId).Id;
                    vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                    vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                    vm.Description = Equery.FindEvent(Pdto.eventId).Description;
                    Pevents.Add(vm);
                }

                UserEventsCreatedList.EventsAttend = Pevents;
                UserEventsCreatedList.EventsCreated = events;


                return View(UserEventsCreatedList);
            }
        }

        [Authorize]
        public ActionResult eventsCreated()
        {
            {
                EventQuery Equery = new EventQuery();
                ParticipantQuery Pquery = new ParticipantQuery();


                allEventsViewModel UserEventsCreatedList = new allEventsViewModel();
                List<EventsViewModel> events = new List<EventsViewModel>();


                List<EventDTO> dtoUserEventsCreated = Equery.QueryUserEventsCreated();

 
                foreach (EventDTO Edto in dtoUserEventsCreated)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Id = Edto.Id;
                    vm.Name = Edto.Name;
                    vm.TimeStart = Edto.TimeStart;
                    vm.Description = Edto.Description;

                    events.Add(vm);
                }

                List<ParticipantsViewModel> Pevents = new List<ParticipantsViewModel>();
                List<ParticipantDTO> ParticipantDTO = Pquery.QueryUserEventsAttending();

                foreach (ParticipantDTO Pdto in ParticipantDTO)
                {
                    ParticipantsViewModel vm = new ParticipantsViewModel();

                    vm.id = Equery.FindEvent(Pdto.eventId).Id;
                    vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                    vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                    vm.Description = Equery.FindEvent(Pdto.eventId).Description;

                    Pevents.Add(vm);
                }

                UserEventsCreatedList.EventsAttend = Pevents;
                UserEventsCreatedList.EventsCreated = events;

                return View(UserEventsCreatedList);
            }
        }


        public PartialViewResult _SocialMediaShare()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CheckIn()
        {
            CheckInsViewModel civm = new CheckInsViewModel();
            return View(civm);
        }

        [HttpPost]
        public ActionResult CheckIn(string rawBarcode)
        {
            EventQuery eq = new EventQuery();
            ParticipantQuery pq = new ParticipantQuery();
            ParticipantDTO pDTO = new ParticipantDTO();
            CheckInQuery ciq = new CheckInQuery();
            CheckInDTO ciDTO = new CheckInDTO();
            CheckInRules ciRules = new CheckInRules();
            BarcodeQuery bcq = new BarcodeQuery();

            bool isValidBarcode = bcq.isValidBarcode(rawBarcode);
            int CheckInTimeCode = 0;
            bool isDuplicate = false;
            string colorCode = "";

            if (isValidBarcode)
            {
                pDTO = pq.FindParticipantByBarcode(rawBarcode);
                CheckInTimeCode = ciRules.IsValidCheckInTime(eq.QueryEventTimes(pDTO.eventId));
                isDuplicate = ciRules.IsDuplicateCheckIn(ciq.QueryCheckInTimes(rawBarcode));
            }

            if (ModelState.IsValid && CheckInTimeCode == 0 && isDuplicate == false && isValidBarcode)
            {
                ciDTO.BarcodeId = bcq.GetBarcodeId(rawBarcode);
                ciq.CreateCheckIn(ciDTO);
                colorCode = "green";
                return Json(new { errorColor = colorCode, error = "Checkin is Successful!" });
            }

            string errorMessage = "";

            if (CheckInTimeCode == -1)
            {
                errorMessage += "The checkin period has not started";
                colorCode = "red";
            }
            else if (CheckInTimeCode == 1)
            {
                errorMessage += "The checkin period has ended";
                colorCode = "red";
            }

            else if (isDuplicate == true)
            {
                errorMessage += "This participant has already checked in";
                colorCode = "yellow";
            }

            else if (!isValidBarcode)
            {
                errorMessage += "This is not a valid barcode";
                colorCode = "red";
            }

            return Json(new { errorColor = colorCode, error = errorMessage });
        }
    }
}



