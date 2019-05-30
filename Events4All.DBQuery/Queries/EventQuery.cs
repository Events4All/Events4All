using Events4All.DB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Data.Entity.Validation;


namespace Events4All.DBQuery
{
    public class EventQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public EventDTO FindEvent(int? id)
        {
            Events events = db.Events.Where( x=> x.Id == id).Where(x=> x.IsActive == true).FirstOrDefault();
            EventDTO dto = MapEventToDTO(events);
            return dto;
        }

        public List<EventDTO> QueryIndexData()
        {
            List<EventDTO> dtoList = new List<EventDTO>();
            List<Events> eventList = db.Events.ToList();

            foreach (Events events in eventList)
            {
                EventDTO dto = MapEventToDTO(events);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public EventDTO MapEventToDTO(Events events)
        {
            EventDTO dto = new EventDTO();
            dto.Address = events.Address;
            dto.Categories = events.Categories;
            dto.City = events.City;
            dto.CreatedDate = events.CreatedDate;
            dto.Description = events.Description;
            dto.Detail = events.Detail;
            dto.HashTag = events.HashTag;
            dto.Id = events.Id;
            dto.IsActive = events.IsActive;
            dto.Logo = events.Logo;
            dto.Name = events.Name;
            dto.State = events.State;
            dto.TicketPrice = events.TicketPrice;
            dto.TimeStart = events.TimeStart;
            dto.TimeStop = events.TimeStop;
            dto.TwitterHandle = events.TwitterHandle;
            dto.Web = events.Web;
            dto.Zip = events.Zip;

            return dto;
        }

        public void CreateEvent(EventDTO EventsDTO)
        {
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
                HashTag = EventsDTO.HashTag
            };

            db.Events.Add(Events);

            //try { db.SaveChanges(); }
            //catch (DbEntityValidationException dbEx)
            //{
            //    new DbEntityValidationException("You must input event dates");
            //}
            db.SaveChanges();
        }
    }
}

        
        public void DeleteConfirmed(int id)
        {
            Events Ev = db.Events.Find(id);
            if(Ev.IsActive == true)
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
            db.SaveChanges();
          
            
        }

    }
}

