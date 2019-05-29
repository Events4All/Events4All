using Events4All.DB.Models;
using System.Collections.Generic;
using System.Linq;




namespace Events4All.DBQuery
{


    public class EventQuery
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public EventDTO FindEvent(int? id)
        {
            Events events = db.Events.Find(id);
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

        
        public void DeleteConfirmed(int id)
        {
            Events Ev = db.Events.Find(id);
            db.Events.Remove(Ev);
            db.SaveChanges();
            
        }

        public void EditEvent(int Id)
        {
            Events Ev = db.Events.Find(Id);
            

              
        }

    }
}

