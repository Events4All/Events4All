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

        [Required(ErrorMessage = "You must enter this field")]
        public string Name { get; set; }

        public ICollection<EventCategories> Categories { get; set; }

        [Required(ErrorMessage = "You must enter this field")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter this field")]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter this field")]
        public string State { get; set; }

        [Required(ErrorMessage = "You must enter this field")]
        public string Zip { get; set; }

        public string Web { get; set; }

        [Display(Name = "Twitter Handle")]
        public string TwitterHandle { get; set; }

        [Display(Name = "Start Time")]
        public DateTime? TimeStart { get; set; }

        [Display(Name = "End Time")]
        public DateTime? TimeStop { get; set; }

        public string Description { get; set; }
        public string Detail { get; set; }
        public byte?[] Logo { get; set; }

        [Display(Name = "Ticket Price")]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Range(0, 1000)]
        public double TicketPrice { get; set; }

        [Display(Name = "Hashtag")]
        public string HashTag { get; set; }
    }
}