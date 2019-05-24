using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Events4All.DB.Models
{
    public class Events : Base
    {
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<EventCategories> Categories { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(100)]
        public string Web { get; set; }

        [StringLength(100)]
        public string TwitterHandle { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeStop { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public string Detail { get; set; }

        public byte?[] Logo { get; set; }

        public double TicketPrice { get; set; }

        [StringLength(50)]
        public string HashTag { get; set; }

    }
}
