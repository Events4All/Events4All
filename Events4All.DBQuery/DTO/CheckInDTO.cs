using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DBQuery.DTO
{
    public class CheckInDTO
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int EventID { get; set; }        
        public int BarcodeId { get; set; }
        public DateTime? CheckInTime { get; set; }
    }
}
