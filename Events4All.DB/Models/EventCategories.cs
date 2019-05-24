using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Events4All.DB.Models
{
    public class EventCategories : Base
    {

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string HashTag { get; set; }

    }
}
