using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events4All.DB.Models;
using Events4All.DBQuery;
using Events4All.Web.Models;


namespace Events4All.Web.Controllers
{
    public class EventsController : Controller
    {
        private EventQuery query = new EventQuery();
        private EventDTO dto = new EventDTO();

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dto = query.FindEvent(id);

            if (dto == null)
            {
                return HttpNotFound();
            }

            EventsViewModel vm = new EventsViewModel();

            vm.Address = dto.Address;
            vm.Categories = dto.Categories;
            vm.City = dto.City;
            vm.CreatedDate = dto.CreatedDate;
            vm.Description = dto.Description;
            vm.Detail = dto.Detail;
            vm.HashTag = dto.HashTag;
            vm.Logo = dto.Logo;
            vm.Id = dto.Id;
            vm.IsActive = dto.IsActive;
            vm.Name = dto.Name;
            vm.State = dto.State;
            vm.TicketPrice = dto.TicketPrice;
            vm.TimeStart = dto.TimeStart;
            vm.TimeStop = dto.TimeStop;
            vm.TwitterHandle = dto.TwitterHandle;
            vm.Web = dto.Web;
            vm.Zip = dto.Zip;

            return View(vm);
        }

        public ActionResult Index2()
        {
            List<EventsViewModel> vmList = new List<EventsViewModel>();
            List<EventDTO> dtoEventList = query.QueryIndexData();

            foreach (EventDTO dto in dtoEventList)
            {
                EventsViewModel vm = new EventsViewModel();

                vm.Address = dto.Address;
                vm.Categories = dto.Categories;
                vm.City = dto.City;
                vm.CreatedDate = dto.CreatedDate;
                vm.Description = dto.Description;
                vm.Detail = dto.Detail;
                vm.HashTag = dto.HashTag;
                vm.Id = dto.Id;
                vm.IsActive = dto.IsActive;
                vm.Logo = dto.Logo;
                vm.Name = dto.Name;
                vm.State = dto.State;
                vm.TicketPrice = dto.TicketPrice;
                vm.TimeStart = dto.TimeStart;
                vm.TimeStop = dto.TimeStop;
                vm.TwitterHandle = dto.TwitterHandle;
                vm.Web = dto.Web;
                vm.Zip = dto.Zip;

                vmList.Add(vm);
            }

            return View(vmList);
        }


        //*****************************************************************
        //Gary's code:
        //*****************************************************************
        // GET: Events/Create
        public ActionResult Create()
        {
            EventsViewModel eventsViewModel = new EventsViewModel();

            return View(eventsViewModel);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,City,State,Zip,Web,TwitterHandle,TimeStart,TimeStop,Description,Detail,TicketPrice,HashTag")] EventsViewModel eventsViewModel)
        {
            //ModelState.Remove("CreatedBy");
            if (ModelState.IsValid)
            {
                //db.Events.Add(events);
                //db.SaveChanges();
                dto.Name = eventsViewModel.Name;
                dto.Address = eventsViewModel.Address;
                dto.City = eventsViewModel.City;
                dto.State = eventsViewModel.State;
                dto.Zip = eventsViewModel.Zip;
                dto.Web = eventsViewModel.Web;
                dto.TwitterHandle = eventsViewModel.TwitterHandle;
                dto.TimeStart = eventsViewModel.TimeStart;
                dto.TimeStop = eventsViewModel.TimeStop;
                dto.Description = eventsViewModel.Description;
                dto.Detail = eventsViewModel.Detail;
                dto.TicketPrice = eventsViewModel.TicketPrice;
                dto.HashTag = eventsViewModel.HashTag;

                query.CreateEvent(dto);
                
                return RedirectToAction("Index2");
            }

            return View(eventsViewModel);
        }
    }
}
