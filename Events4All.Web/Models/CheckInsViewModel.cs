using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events4All.Web.Models
{
    public class CheckInsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Participant Id")]
        public int ParticipantId { get; set; }

        public string UserId { get; set; }
        public int EventId { get; set; }
        public DateTime? CheckInTime { get; set; }

        [Display(Name = "Raw Barcode")]
        public string RawBarcode { get; set; }
    }
}