using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DB.Models
{
    public class CheckIns : Base
    {        
        public DateTime? CheckinTime { get; set; }
    }
}
