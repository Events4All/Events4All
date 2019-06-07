using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;
using Events4All.DBQuery.DTO;
using Events4All.DB.Models;

namespace Events4All.DBQuery.Queries
{
    public class ReminderQuery
    {
        //Instantiates the database object
        public ApplicationDbContext db = new ApplicationDbContext();

        /* Method to get the participants that have elected to have reminder notifications sent to them
                -queries the database and gets the results as a list
                -passes the list into a DTO object and returns a DTO with the query results */
        public List<ReminderDTO> GetReminderData()
        {
            /* LINQ syntax to query the database for participants with reminder notifications set for today
                    -creates a list from the query results */
            var reminderList = (from u in db.Users
                                join e in db.Events on u.Id equals e.CreatedBy.Id
                                join p in db.Participants on e.Id equals p.EventID.Id
                                where p.emailNotificationOn == true
                                where p.Reminder.Value.Year == DateTime.Now.Year
                                where p.Reminder.Value.Month == DateTime.Now.Month
                                where p.Reminder.Value.Day == DateTime.Now.Day

                                select new
                                {
                                    u.Email,
                                    e.Name,
                                    e.Address,
                                    e.City,
                                    e.State,
                                    e.Zip,
                                    e.TimeStart,
                                    e.TimeStop
                                }).ToList();

            //Instantiates a List type based on the Reminder DTO object found in the DTO folder
            List<ReminderDTO> reminderDTOList = new List<ReminderDTO>();

            /* Iterates the query result list and sets the DTO field value to the query field value
                    -then adds the complete record to the DTO object
                    -finally returns the DTO object with the query data */
            foreach (var u in reminderList)
            {
                //Instantiates a new Reminder DTO object found in the DTO folder
                ReminderDTO reminderDTO = new ReminderDTO();

                reminderDTO.Email = u.Email;
                reminderDTO.Name = u.Name;
                reminderDTO.Address = u.Address;
                reminderDTO.City = u.City;
                reminderDTO.State = u.State;
                reminderDTO.Zip = u.Zip;
                reminderDTO.TimeStartShort = Convert.ToDateTime(u.TimeStart).ToShortDateString();
                reminderDTO.TimeStart12Hr = Convert.ToDateTime(u.TimeStart).ToShortTimeString();
                reminderDTO.TimeStopShort = Convert.ToDateTime(u.TimeStop).ToShortDateString();
                reminderDTO.TimeStop12Hr = Convert.ToDateTime(u.TimeStop).ToShortTimeString();

                reminderDTOList.Add(reminderDTO);
            }

            return reminderDTOList;

        }
    }
}
