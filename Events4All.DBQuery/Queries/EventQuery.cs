using Events4All.DB.Models;
using Events4All.DBQuery.Queries;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Events4All.DBQuery
{
    public class EventQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public EventDTO FindEvent(int? id)
        {
            Events events = db.Events.Where(x => x.Id == id).Where(x => x.IsActive == true).FirstOrDefault();
            EventDTO eventDto = MapEventToDTO(events);
            return eventDto;
        }

        public List<EventDTO> QueryIndexData()
        {
            List<EventDTO> eventDtoList = new List<EventDTO>();
            List<Events> eventList = db.Events.ToList();


            foreach (Events events in eventList)
            {
                EventDTO eventDto = MapEventToDTO(events);
                eventDtoList.Add(eventDto);
            }

            return eventDtoList;
        }

        public EventDTO MapEventToDTO(Events events)
        {
            EventDTO eventDto = new EventDTO();
            eventDto.Address = events.Address;
            eventDto.Categories = events.Categories;
            eventDto.City = events.City;
            eventDto.CreatedDate = events.CreatedDate;
            eventDto.Description = events.Description;
            eventDto.Detail = events.Detail;
            eventDto.HashTag = events.HashTag;
            eventDto.Id = events.Id;
            eventDto.IsActive = events.IsActive;
            eventDto.Logo = events.Logo;
            eventDto.Name = events.Name;
            eventDto.State = events.State;
            eventDto.TicketPrice = events.TicketPrice;
            eventDto.TimeStart = events.TimeStart;
            eventDto.TimeStop = events.TimeStop;
            eventDto.TwitterHandle = events.TwitterHandle;
            eventDto.Web = events.Web;
            eventDto.Zip = events.Zip;
            eventDto.AttendeeCap = events.AttendeeCap;

            return eventDto;
        }

        public void CreateEvent(EventDTO EventsDTO)
        {
            //try
            //{
                string userId = HttpContext.Current.User.Identity.GetUserId();
                ApplicationUser user = db.Users.Find(userId);

            var Events = new Events
            {
                CreatedBy = user,
                IsActive = true,
                CreatedDate = DateTime.Now,
                Name = EventsDTO.Name,
                Categories = EventsDTO.Categories,
                Address = EventsDTO.Address,
                City = EventsDTO.City,
                State = EventsDTO.State,
                Zip = EventsDTO.Zip,
                Web = EventsDTO.Web,
                TwitterHandle = EventsDTO.TwitterHandle,
                TimeStart = EventsDTO.TimeStart,
                TimeStop = EventsDTO.TimeStop,
                Description = EventsDTO.Description,
                Detail = EventsDTO.Detail,
                Logo = EventsDTO.Logo,
                TicketPrice = EventsDTO.TicketPrice,
                HashTag = EventsDTO.HashTag,
                AttendeeCap = EventsDTO.AttendeeCap
            };

                db.Events.Add(Events);
                db.SaveChanges();
//        }
//            catch (DbEntityValidationException e)
//            {
//                foreach (var eve in e.EntityValidationErrors)
//                {
//                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);

//                    foreach (var ve in eve.ValidationErrors)
//                    {
//                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",  ve.PropertyName, ve.ErrorMessage);
//                    }
//}
//                throw;
//            }
        }


        public void DeleteConfirmed(int id)
        {
            Events Ev = db.Events.Find(id);
            if (Ev.IsActive == true)
            {
                Ev.IsActive = false;
            }

            db.SaveChanges();

        }

        public void EditEvent(EventDTO DT)
        {
            Events Ev = db.Events.Find(DT.Id);
            Ev.Name = DT.Name;
            Ev.Address = DT.Address;
            Ev.City = DT.City;
            Ev.State = DT.State;
            Ev.Zip = DT.Zip;
            Ev.Categories = DT.Categories;
            Ev.Description = DT.Description;
            Ev.Logo = DT.Logo;
            Ev.Detail = DT.Detail;
            Ev.TimeStart = DT.TimeStart;
            Ev.TimeStop = DT.TimeStop;
            Ev.HashTag = DT.HashTag;
            Ev.TicketPrice = DT.TicketPrice;
            Ev.TwitterHandle = DT.TwitterHandle;
            Ev.Web = DT.Web;
            Ev.AttendeeCap = DT.AttendeeCap;
            db.SaveChanges();
        }

        public List<EventDTO> QueryUserEventsCreated()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            List<EventDTO> eventDtoList = new List<EventDTO>();
            List<Events> eventList = db.Events.Where(i => i.CreatedBy.Id == userId).Where(x => x.IsActive == true).ToList();

            foreach (Events userEvents in eventList)
            {
                EventDTO eventDto = MapEventToDTO(userEvents);
                eventDtoList.Add(eventDto);
            }

            return eventDtoList;
        }

        public DateTime[] QueryEventTimes(int eventId)
        {
            EventDTO eDTO = new EventDTO();
            eDTO = FindEvent(eventId);
            DateTime checkinStart = eDTO.TimeStart.Value;
            DateTime checkinEnd = eDTO.TimeStop.Value;
            DateTime[] eventTimes = { checkinStart, checkinEnd };
            return eventTimes;
        }

        public int GetRemainingTickets(int eventId)
        {
            BarcodeQuery bq = new BarcodeQuery();

            int attendeeCap = FindEvent(eventId).AttendeeCap;
            int attendees = bq.GetNumberOfAttendees(eventId);
            int remainingTickets = attendeeCap - attendees;
            return remainingTickets;
        }
    }
}


