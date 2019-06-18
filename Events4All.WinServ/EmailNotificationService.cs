using Events4All.DBQuery.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace Events4All.WinServ
{
    public partial class EmailNotificationService : ServiceBase
    {
        public EmailNotificationService()
        {
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                DBQuery.Queries.ReminderQuery rq = new DBQuery.Queries.ReminderQuery();
                List<ReminderDTO> reminderDTOList = new List<ReminderDTO>();
                reminderDTOList = rq.GetReminderData();

                foreach (ReminderDTO dto in reminderDTOList)
                {
                    ReminderDTO reminderDTO = new ReminderDTO();

                    string eventId = dto.EventId;
                    string participantId = dto.ParticipantId;
                    string email = dto.Email;
                    string eventName = dto.Name;
                    string address = dto.Address;
                    string city = dto.City;
                    string state = dto.State;
                    string zip = dto.Zip;
                    string startDate = dto.TimeStartShort;
                    string startTime = dto.TimeStart12Hr;
                    string endDate = dto.TimeStopShort;
                    string endTime = dto.TimeStop12Hr;

                    string adminEmail = "samnasr@live.com";

                    //List of participant email address along with admin email address
                    List<string> emailAddresses = new List<string>();
                    emailAddresses.Add(email);
                    emailAddresses.Add(adminEmail);

                    Business.ENotifications.EmailNotification emailNotification = new Business.ENotifications.EmailNotification();

                    string body = ($"This is a reminder: " + "<br />" + "<br />" +
                                    $"You're scheduled to attend the event <b>{eventName}</b> on <b>{startDate}</b>" +
                                    $" from <b>{startTime}</b>" +
                                    $" to <b>{endTime}</b>. " + "<br />" + "<br />" + 
                                    $"The location is: " + " <br /> " + " <br /> " + 
                                    $"<b>{address}</b>, <b>{city}</b>, <b>{state}</b> <b>{zip}</b>");

                    string subject = "Event Reminder";
                    //emailNotification.SendEmail(email, body, subject);

                    foreach (string emailAddress in emailAddresses)
                    {
                        emailNotification.SendEmail(emailAddress, body, subject);
                    }

                    /* Timestamps when the email notification was sent 
                            -updates the current participant record with the current datetime
                            -the reminder query is set to pull where the EmailNotificationSentTime 
                       field is null (to prevent sending duplicate notifications) */
                    int participantKey = Int32.Parse(participantId);
                    var participants = rq.db.Participants.Find(participantKey);
                    participants.EmailNotificationSentTime = DateTime.Now;
                    rq.db.Participants.Attach(participants);
                    var entry = rq.db.Entry(participants);
                    entry.Property(e => e.EmailNotificationSentTime).IsModified = true;
                    rq.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                WriteToFile($"Message={ex.Message}\n InnerException={ex.InnerException} \n\n");
            }
        }

        /* Creates a text file where a string can be passed in:
          -used to pass in any error messages (Windows Service doesn't have debugging functionality)
          -see Exception Handling above
          -can also be placed at different points in the code to display a test message as a way to see 
           if the Windows Service is firing blocks of code successfully e.g., WriteToFile("Testing") 
        */
        private void WriteToFile(string text)
        {
            string path = "C:\\CC7\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + ":" + text);
                writer.Close();
            }
        }

        protected override void OnStop()
        {
            //eventLog1.WriteEntry("Stopping the service", EventLogEntryType.Information, 1001);
        }

        protected override void OnPause()
        {
            //eventLog1.WriteEntry("Pausing the service", EventLogEntryType.Information, 1001);
        }

        protected override void OnContinue()
        {
            //eventLog1.WriteEntry("Continuing the service", EventLogEntryType.Information, 1001);
        }

        protected override void OnShutdown()
        {
            //eventLog1.WriteEntry("Shutting down the service", EventLogEntryType.Information, 1001);
        }

    }
}
