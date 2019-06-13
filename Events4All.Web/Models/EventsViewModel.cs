using Events4All.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Events4All.Web.Models
{
    public class EventsViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name = "Event Name")]
        [Required(ErrorMessage = "You must enter {0}")]        
        public string Name { get; set; }

        public ICollection<EventCategories> Categories { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public string State { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string Zip { get; set; }

        public string Web { get; set; }

        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; set; }

        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString ="{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime? TimeStart { get; set; }

        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime? TimeStop { get; set; }

        public string Description { get; set; }
        public string Detail { get; set; }
        public byte?[] Logo { get; set; }
        public String Number { get; set; }
        public string Street { get; set; }

        [Display(Name = "Ticket Price")]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Range(0, 1000)]
        public double TicketPrice { get; set; }

        [Display(Name = "Hashtag")]
        public string HashTag { get; set; }

        [Display(Name = "Attendee Cap")]
        public int AttendeeCap { get; set; }

       // public string CreatedBy { get; set; }
    }
}