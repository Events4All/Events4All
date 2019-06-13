using Events4All.DB.Models;
using Events4All.DBQuery.DTO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Events4All.DBQuery.Queries
{
    public class CheckInQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void CreateCheckIn(CheckInDTO checkInDTO)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);

            Barcodes barcode = db.Barcodes.Find(checkInDTO.BarcodeId);
            CheckIns ci = new CheckIns();

            ci.CheckinTime = DateTime.Now;
            ci.CreatedDate = DateTime.Now;
            ci.CreatedBy = user;
            ci.IsActive = true;
            
            db.CheckIns.Add(ci);
            barcode.CheckIns = new List<CheckIns>();
            barcode.CheckIns.Add(ci);
            db.SaveChanges();
        }

        public List<DateTime> QueryCheckInTimes(string guid)
        {
            List<DateTime> checkInTimes = new List<DateTime>();

            Barcodes barcode = db.Barcodes
                .Include(x => x.CheckIns)
                .Where(x => x.Barcode.ToString() == guid && x.IsActive == true)
                .SingleOrDefault();

            foreach (CheckIns checkIn in barcode.CheckIns)
            {
                checkInTimes.Add(checkIn.CheckinTime.Value);
            }

            return checkInTimes;
        }
    }
}
