using Events4All.Business.ENotifications;
using Events4All.Constants;
using Events4All.DBQuery;
using Events4All.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Events4All.Business;


namespace Events4All.Web.Controllers
{
    public class ParticipantsController : Controller
    {
        [HttpGet]
        public ActionResult Create(int id)
        {         
            ParticipantsViewModel vm = new ParticipantsViewModel();
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDto = new EventDTO();

            eventDto = eventQuery.FindEvent(id);
            vm.TicketPrice = eventDto.TicketPrice;

            return View(vm);
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NumberOfTicket, Reminder")] ParticipantsViewModel participantsViewModel, int id)
        {
            ParticipantDTO participantDto = new ParticipantDTO();
            ParticipantQuery participantQuery = new ParticipantQuery();

            //Gary added to find ticket price
            EventDTO eventDTO = new EventDTO();
            EventQuery eventQuery = new EventQuery();
            eventDTO.TicketPrice = eventQuery.FindEvent(id).TicketPrice;

            if (ModelState.IsValid)
            {
                //Gary added if statement for payment screen
                if ((participantsViewModel.NumberOfTicket > 0) && (eventDTO.TicketPrice > 0))
                {
                    TempData["Reminder"] = participantsViewModel.Reminder;
                    TempData["NumberOfTickets"] = participantsViewModel.NumberOfTicket;
                    TempData["TicketPrice"] = eventDTO.TicketPrice;
                    TempData["Subtotal"] = Convert.ToInt32(participantsViewModel.NumberOfTicket) *
                        Convert.ToInt32(eventDTO.TicketPrice);

                    return RedirectToAction("Payment/" + id, "Participants");
                }
                else
                {
                    participantDto.NumberOfTicket = participantsViewModel.NumberOfTicket;
                    participantDto.Reminder = participantsViewModel.Reminder;
                    participantDto.eventId = id;
                    participantDto.Barcodes = new List<Guid>();

                    for (int i = 0; i < participantDto.NumberOfTicket; i++)
                    {
                        Guid barcode = Guid.NewGuid();
                        participantDto.Barcodes.Add(barcode);
                    }

                    int participantID = participantQuery.CreateParticipant(participantDto);

                //EventDTO eventDTO = new EventDTO();
                ParticipantsViewModel vm = new ParticipantsViewModel();
                //EventQuery eventQuery = new EventQuery();
                UserDTO userDTO = new UserDTO();
                UserQuery userQuery = new UserQuery();
                userDTO = userQuery.FindCurrentUser();
                eventDTO = eventQuery.FindEvent(participantDto.eventId);

                string content = System.IO.File.ReadAllText(Server.MapPath("~/ConfirmMail.cshtml"));
                content = content.Replace("{{Name}}", eventDTO.Name);
                content = content.Replace("{{Address}}", eventDTO.Address);
                content = content.Replace("{{City}}", eventDTO.City);
                content = content.Replace("{{State}}", eventDTO.State);
                content = content.Replace("{{Zip}}", eventDTO.Zip.ToString());
                content = content.Replace("{{TimeStart}}", eventDTO.TimeStart.ToString());
                content = content.Replace("{{Ticket}}", participantDto.NumberOfTicket.ToString());
                content = content.Replace("{{participantID}}", participantID.ToString());
                content = content.Replace("{{eventID}}", eventDTO.Id.ToString());
                var toEmail = userDTO.Username.ToString();
                new EmailNotification().SendEmail(toEmail, content, "Confirmation : You have registered for an event!");

                return RedirectToAction("RegistrationConfirmation/" + participantID, "Participants");
            }

            }

            return View(participantsViewModel);
        }

        [HttpGet]
        public ActionResult RegistrationConfirmation(int id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            ParticipantDTO participantDTO = new ParticipantDTO();
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDTO = new EventDTO();

            ParticipantsViewModel vm = new ParticipantsViewModel();

            //Gary added to give payment confirmation
            string confirmCode = "";
            if (TempData.ContainsKey("ConfirmationCode"))
                confirmCode = TempData["ConfirmationCode"].ToString();
            if (confirmCode == "")
            {
                ViewBag.Confirmation = "";
            }
            else
            {
                ViewBag.Confirmation = "The payment processed successfully.  " +
                        "Your confirmation number is " + confirmCode;
            }
            
            participantDTO = participantQuery.FindParticipant(id);
            eventDTO = eventQuery.FindEvent(participantDTO.eventId);

            vm.EventName = eventDTO.Name;
            vm.NumberOfTicket = participantDTO.NumberOfTicket;
            vm.EventStartDate = eventDTO.TimeStart;
            vm.Barcodes = participantDTO.Barcodes;
            vm.Address = eventDTO.Address;
            vm.City = eventDTO.City;
            vm.State = eventDTO.State;
            vm.Zip = eventDTO.Zip;

            ViewBag.EventId = eventDTO.Id;

            ViewBag.Location = ($"{vm.Address}, {vm.City}, {vm.State} {vm.Zip}");

            return View(vm);
        }

        public ActionResult RegistrationConfirmationPrint(int id)
        {
            ParticipantQuery participantQuery = new ParticipantQuery();
            ParticipantDTO participantDTO = new ParticipantDTO();
            EventQuery eventQuery = new EventQuery();
            EventDTO eventDTO = new EventDTO();
            UserDTO userDTO = new UserDTO();
            UserQuery userQuery = new UserQuery();

            ParticipantsViewModel vm = new ParticipantsViewModel();

            participantDTO = participantQuery.FindParticipant(id);
            eventDTO = eventQuery.FindEvent(participantDTO.eventId);
            userDTO = userQuery.FindCurrentUser();

            vm.EventName = eventDTO.Name;
            vm.NumberOfTicket = participantDTO.NumberOfTicket;
            vm.EventStartDate = eventDTO.TimeStart;

            ViewBag.NumberOfTickets = participantDTO.NumberOfTicket;
            ViewBag.Username = userDTO.Username.Replace("@", "").Replace(".com", "");
            ViewBag.UserId = userDTO.Id;
            ViewBag.EventName = eventDTO.Name;
            ViewBag.EventId = eventDTO.Id;

            return View(vm);
        }

        [HttpGet]
        public ActionResult Reminders(int id)
        {
            ViewBag.PhoneValidation = ConstantValues.phoneValidation;
            RemindersViewModel rvm = new RemindersViewModel();
            ParticipantQuery pq = new ParticipantQuery();
            EventQuery eq = new EventQuery();

            ParticipantDTO pDTO = pq.FindParticipant(id);
            EventDTO evDTO = eq.FindEvent(pDTO.eventId);

            //Map DTO fields to rvm            
            rvm.EventStartDate = evDTO.TimeStart;
            rvm.Reminder = pDTO.Reminder;
            rvm.emailNotificationOn = pDTO.emailNotificationOn;
            rvm.SMSNotificationOn = pDTO.SMSNotificationOn;
             
            return View(rvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reminders(int id, [Bind(Include ="Reminder, emailNotificationOn, SMSNotificationOn, TimeStart, PhoneNumber")] RemindersViewModel remindersViewModel)
        {
            

            if(id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(ModelState.IsValid)
            {
                ParticipantQuery pq = new ParticipantQuery();
                ParticipantDTO pDTO = new ParticipantDTO();
                
                pDTO.Reminder = remindersViewModel.Reminder;
                pDTO.emailNotificationOn = remindersViewModel.emailNotificationOn;
                pDTO.SMSNotificationOn = remindersViewModel.SMSNotificationOn;
                pDTO.Id = id;
                pDTO.Phone = remindersViewModel.PhoneNumber;

                pq.UpdateParticipantReminders(pDTO);
                
                 return RedirectToAction("ReminderConfirmation", "Participants");
            }

            return View(remindersViewModel);
        }


        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult ReminderConfirmation()
        {
            return View();
        }

        public ActionResult Payment(int id)
        {
            ParticipantsViewModel participantsViewModel = new ParticipantsViewModel();

            if ((TempData.ContainsKey("Reminder")) && (TempData["Reminder"] != null))
                participantsViewModel.Reminder = DateTime.Parse(TempData["Reminder"].ToString());

            if (TempData.ContainsKey("NumberOfTickets"))
                participantsViewModel.NumberOfTicket = int.Parse(TempData["NumberOfTickets"].ToString());

            if (TempData.ContainsKey("TicketPrice"))
                participantsViewModel.TicketPrice = double.Parse(TempData["TicketPrice"].ToString());

            if (TempData.ContainsKey("Subtotal"))
                participantsViewModel.Subtotal = double.Parse(TempData["Subtotal"].ToString());

            TempData.Keep("Reminder");
            TempData.Keep("NumberOfTickets");
            TempData.Keep("TicketPrice");
            TempData.Keep("Subtotal");

            return View(participantsViewModel);
        }

        //Gary added for Payment view
        [HttpPost]
        public ActionResult Payment([Bind(Include = "NumberOfTicket, Reminder")] ParticipantsViewModel participantsViewModel, int id)
        {
            ParticipantDTO participantDto = new ParticipantDTO();
            ParticipantQuery participantQuery = new ParticipantQuery();
            UserQuery userQuery = new UserQuery();
            EPay ePay = new EPay();

            if ((TempData.ContainsKey("Reminder")) && (TempData["Reminder"] != null))
                participantsViewModel.Reminder = DateTime.Parse(TempData["Reminder"].ToString());
                //participantsViewModel.NumberOfTicket = int.Parse(TempData["Reminder"].ToString());

            if (TempData.ContainsKey("NumberOfTickets"))
                participantsViewModel.Subtotal = int.Parse(TempData["NumberOfTickets"].ToString());

            if (TempData.ContainsKey("Subtotal"))
                participantsViewModel.Subtotal = double.Parse(TempData["Subtotal"].ToString());

            double purchAmt = participantsViewModel.Subtotal * 100;
            string purchEmail = userQuery.FindCurrentUser().Username;

            if (ModelState.IsValid)
            {
                //Web Api call to Stripe Credit Card Payment Service
                string apiResp = ePay.MakeStripeApiRequest(purchAmt, purchEmail);
                string confirmCode = apiResp.Substring(0, apiResp.IndexOf(','));
                string statusCode = apiResp.Substring(apiResp.IndexOf(',') + 1, apiResp.Length - (apiResp.IndexOf(',') + 1));

                if(statusCode.ToUpper() == "OK")
                {
                    TempData["ConfirmationCode"] = confirmCode;
                    TempData.Keep("ConfirmationCode");
                    ViewBag.Confirmation = "The payment processed successfully.  " +
                        "Your confirmation number is " + confirmCode;
                    participantDto.NumberOfTicket = participantsViewModel.NumberOfTicket;
                    participantDto.Reminder = participantsViewModel.Reminder;
                    participantDto.eventId = id;
                    participantDto.Barcodes = new List<Guid>();

                    for (int i = 0; i < participantDto.NumberOfTicket; i++)
                    {
                        Guid barcode = Guid.NewGuid();
                        participantDto.Barcodes.Add(barcode);
                    }

                    int participantID = participantQuery.CreateParticipant(participantDto);

                    return RedirectToAction("RegistrationConfirmation/" + participantID, "Participants");                    
                }
                else
                {
                    ViewBag.Confirmation = "The payment failed.  Please try again.";
                }
            }

            return View(participantsViewModel);
        }
    }
}

