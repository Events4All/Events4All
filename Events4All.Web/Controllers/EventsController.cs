using Events4All.DBQuery;
using Events4All.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Events4All.DB.Models;
using Events4All.DBQuery;
using Events4All.Web.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;

namespace Events4All.Web.Controllers
{
    public class EventsController : Controller
    {
        //private EventQuery Equery = new EventQuery();
        //private EventDTO Edto = new EventDTO();

        //private ParticipantQuery participantQuery = new ParticipantQuery();

        //private ApplicationDbContext db = new ApplicationDbContext();


        //private ParticipantQuery Pquery = new ParticipantQuery();
        //private ParticipantDTO Pdto = new ParticipantDTO();

        public ActionResult Details(int? id)
        {
             ParticipantQuery participantQuery = new ParticipantQuery();
             EventQuery Equery = new EventQuery();
             EventDTO Edto = new EventDTO();

            if (participantQuery.IsRegistered(id))
            {
                ViewBag.Registered = "You have already registered for this event.";
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

            return View(vm);
        }

        public ActionResult Index2()
        {
            EventQuery Equery = new EventQuery();

            List<EventsViewModel> vmList = new List<EventsViewModel>();
            List<EventDTO> dtoEventList = Equery.QueryIndexData();

            foreach (EventDTO dto in dtoEventList)
            {
                if (dto.IsActive == true)
                {
                    EventsViewModel vm = new EventsViewModel();

                    vm.Address = dto.Address;
                    vm.Categories = dto.Categories;
                    vm.City = dto.City;
                    vm.CreatedDate = dto.CreatedDate;
                    vm.Description = dto.Description;
                    vm.Detail = dto.Detail;
                    vm.HashTag = dto.HashTag;
                    vm.Id = dto.Id;
                    vm.IsActive = dto.IsActive;
                    vm.Logo = dto.Logo;
                    vm.Name = dto.Name;
                    vm.State = dto.State;
                    vm.TicketPrice = dto.TicketPrice;
                    vm.TimeStart = dto.TimeStart;
                    vm.TimeStop = dto.TimeStop;
                    vm.TwitterHandle = dto.TwitterHandle;
                    vm.Web = dto.Web;
                    vm.Zip = dto.Zip;

                    vmList.Add(vm);
                }
            }

            return View(vmList);
        }

        //*****************************************************************
        //Gary's code:
        //*****************************************************************
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
        public ActionResult Create([Bind(Include = "Name,Address,City,State,Zip,Web,TwitterHandle,TimeStart,TimeStop,Description,Detail,TicketPrice,HashTag")] EventsViewModel eventsViewModel)
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

                Equery.CreateEvent(Edto);

                return RedirectToAction("Index2");
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

            Equery.EditEvent(Edto);
            return RedirectToAction("Index2");
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
           

            return View(vm);

        }
        
        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            EventQuery Equery = new EventQuery();

            Equery.DeleteConfirmed(id);
            return RedirectToAction("Index2");
        }


        public ActionResult allEvents()
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

                vm.EventName = Equery.FindEvent(Pdto.eventId).Name;
                vm.EventStartDate = Equery.FindEvent(Pdto.eventId).TimeStart;
                vm.Description = Equery.FindEvent(Pdto.eventId).Description;

                Pevents.Add(vm);
            }

            UserEventsCreatedList.EventsAttend = Pevents;
            UserEventsCreatedList.EventsCreated = events;

            return View(UserEventsCreatedList);
        }

        public PartialViewResult SelectCalendarType(int id)
        {
            ViewBag.EventId = id;
            return PartialView();
        }

        public void DownloadCalendar(int id)
        {
            EventQuery query = new EventQuery();
            EventDTO dto = query.FindEvent(id);
            byte[] ics = GenerateICSFile(dto);
            Response.Clear();
            Response.ContentType = "text/calendar";
            Response.AddHeader("content-disposition", "attachment: filename=" +"event.ics");
            Response.BinaryWrite(ics);
            Response.End();
        }

        public byte[] GenerateICSFile(EventDTO dto)
        {
            var calendar = new Calendar();

            calendar.Events.Add(new CalendarEvent
            {
                Class = "PUBLIC",
                Summary = dto.Name + " - " + dto.Description,
                Created = new CalDateTime(DateTime.Now),
                Description = dto.Detail,
                Start = new CalDateTime(dto.TimeStart.Value),
                End = new CalDateTime(dto.TimeStop.Value),
                Sequence = 0,
                Uid = Guid.NewGuid().ToString(),
                Location = dto.Address + " " + dto.City + " " + dto.State + " " + dto.Zip,
            });

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);
            byte[] calendarBytes = System.Text.Encoding.UTF8.GetBytes(serializedCalendar);
            return calendarBytes;
        }
    }
}
