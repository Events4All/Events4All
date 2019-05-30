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
                if (dto.IsActive == true)
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
            }

            return View(vmList);
        }

        //get
        public ActionResult Edit(int? id)
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
            vm.Zip = dto.Zip;
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
        

            return View(vm);
        }
    
        //post
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventsViewModel EVM)
        {
            dto.Id = EVM.Id;
            dto.Name = EVM.Name;
            dto.Address = EVM.Address;
            dto.City = EVM.City;
            dto.State = EVM.State;
            dto.Zip = EVM.Zip;
            dto.Detail = EVM.Detail;
            dto.Categories = EVM.Categories;
            dto.Description = EVM.Description;
            dto.Logo = EVM.Logo;
            dto.TicketPrice = EVM.TicketPrice;
            dto.TimeStart = EVM.TimeStart;
            dto.TimeStop = EVM.TimeStop;
            dto.HashTag = EVM.HashTag;
            dto.TwitterHandle = EVM.TwitterHandle;
            dto.Web = EVM.Web;

            query.EditEvent(dto);
            return RedirectToAction("Index2");
        }
        



        // GET
        public ActionResult Delete(int? id)
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
            vm.Description = dto.Description;
            vm.Detail = dto.Detail;
            vm.Zip = dto.Zip;
            vm.HashTag = dto.HashTag;
            vm.Id = dto.Id;
            vm.IsActive = dto.IsActive;
            vm.Logo = dto.Logo;
            vm.Name = dto.Name;
            vm.State = dto.State;
            vm.CreatedDate = dto.CreatedDate;
            vm.TicketPrice = dto.TicketPrice;
            vm.TimeStart = dto.TimeStart;
            vm.TimeStop = dto.TimeStop;
            vm.TwitterHandle = dto.TwitterHandle;
            vm.Web = dto.Web;
           

            return View(vm);

        }// POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            query.DeleteConfirmed(id);
            return RedirectToAction("Index2");
        }

       
    }
}
