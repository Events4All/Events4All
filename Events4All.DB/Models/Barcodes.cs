using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DB.Models
{
    public class Barcodes : Base
    {
        public Guid Barcode { get; set; }
        public ICollection<CheckIns> CheckIns { get; set; }
    }
}
