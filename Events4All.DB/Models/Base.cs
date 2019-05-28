using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events4All.DB.Models
{
    public class Base
    {
        public int Id { get; set; }

        [Required]
        public Boolean IsActive { get; set; }

        [Required]
        public ApplicationUser CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        //public ApplicationUser ModifiedBy { get; set; }

        //public DateTime? ModifiedDate { get; set; }

    }
}
