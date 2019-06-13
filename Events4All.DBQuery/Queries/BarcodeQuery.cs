using Events4All.DB.Models;
using System.Data.Entity;
using System.Linq;

namespace Events4All.DBQuery.Queries
{
    public class BarcodeQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int GetBarcodeId(string guid)
        {
            Barcodes barcode = db.Barcodes
                .Include(x => x.CheckIns)
                .Where(x => x.Barcode.ToString() == guid && x.IsActive == true)
                .SingleOrDefault();

            int barcodeId = barcode.Id;
            return barcodeId;
        }

        public bool isValidBarcode(string guid)
        {
            bool valid = false;

            Barcodes barcode = db.Barcodes
                .Include(x => x.CheckIns)
                .Where(x => x.Barcode.ToString() == guid && x.IsActive == true)
                .SingleOrDefault();

            if(barcode != null)
            {
                valid = true;
            }

            return valid;
        }
    }
}
