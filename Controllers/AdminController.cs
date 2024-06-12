using Busticket.Models;
using Busticket.programs;
using Busticket.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Busticket.Controllers
{
    public class AdminController : Controller
    {
        private readonly vendorRepository _vendorRepository;
        private readonly BusRepository _busRepository;
        private readonly SeatRepository _seatRepository;

        public AdminController()
        {
            this._vendorRepository = new vendorRepository(new DbBooking());
            this._busRepository = new BusRepository(new DbBooking());
            this._seatRepository = new SeatRepository(new DbBooking());
        }


        // GET: Vendor
        public ActionResult AdminIndex()
        {
            List<Vendor> ven = _vendorRepository.GetAll();

            return View(ven);
        }

        public ActionResult VendorBusses(int id)
        {
            Session.Add("vendorId",id);
            List<Bus> buses = _busRepository.VendorBusesList(id);
            return View(buses);
        }


        public ActionResult CreateVendor()
        {
            Vendor ven = new Vendor();
            UniqueRandomNumberGenerator generator = new UniqueRandomNumberGenerator();
            ven.id = generator.GenerateUniqueRandomNumber();
            return View(ven);
        }

        [HttpPost]
        public ActionResult CreateVendor(Vendor ven)
        {
            if(ModelState.IsValid)
            {
                if (_vendorRepository.add(ven))
                {
                    return RedirectToAction("VendorPassword", new { vendorId = ven.id });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View(ven);
            }
        }

        public ActionResult VendorPassword(int vendorId)
        {
            ViewBag.VendorId = vendorId; // Passing passengerId to the view
            return View();
        }

        [HttpPost]
        public ActionResult VendorPassword(int vendorId, string password1, string password2)
        {

            if (password1 == null || password2 == null)
                return View("Error");

            if (password1 != password2)
                return Content("Provided passwords do not match");

            bool passwordChanged = _vendorRepository.VendorPassword(vendorId, password1);

            if (passwordChanged)
            {
                return RedirectToAction("AdminIndex");
            }
            else
            {
                return View("Error");
            }
        }




        public ActionResult CreateBus()
        {
            
            Bus bus = new Bus();
            bus.vendorid = (int)Session["vendorId"];
            return View(bus);
        }

        // POST: Bus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBus(Bus bus)
        {
            if (ModelState.IsValid)
            {
                if (_busRepository.add(bus))
                {
                    return RedirectToAction("VendorBusses",new {id = bus.vendorid});
                }

                else
                {
                    return View("Error");
                }

            }
            else
            {

                return View(bus);
            }

        }

            // GET: Vendor/Details/5

        
        public ActionResult AllBus()
        {
            List<Bus> busess = _busRepository.GetAll();
            return View(busess);
        }


        public ActionResult DetailsBus(string id)
        {
            object a = (object)id;
            Bus bus = _busRepository.find(a);
            int emptySeats = _busRepository.TotalSeatAvailable(id);
            ViewBag.EmptySeats = emptySeats;
            return View(bus);
        }

        public ActionResult BusLogs(string busNo)
        {


            List<BusLog> busLogs = _seatRepository.busLogList(busNo);
            List<int> busLogseats = _seatRepository.BuslogSeatAvailable(busNo);
            ViewBag.seats = busLogseats;
            return View(busLogs);
        }


        public ActionResult DetailsVen(int id)
        {
            object a = (object)id;
            Session.Add("vendorId", id);
                Vendor ven = _vendorRepository.find(a);
            return View(ven);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult EditBus(string id)
        {
            object obj = (object)id;
            Bus bu = _busRepository.find(obj);
            return View(bu);
        }


        [HttpPost]
        public ActionResult EditBus(string id, Bus bu)
        {
            object obj = (object)(id);
            Bus bus = _busRepository.find(obj);
            if (ModelState.IsValid)
            {
                
                bool b = _busRepository.UpdateBus(id, bu);
                if (b)
                {
                    return RedirectToAction("VendorBusses", new { id = bus.vendorid});
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View(bu);
            }

        }
        [HttpGet]
        // GET: Vendor/Edit/5
        public ActionResult EditVen(int id)
        {
            object v = (object)id;  
            Vendor ven = _vendorRepository.find(v);
            return View(ven);
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        public ActionResult EditVen(int id,Vendor ven)
        {
           if(ModelState.IsValid)
            {
                object a = (object)id;
                bool b = _vendorRepository.update(a, ven);
                if(b)
                {
                    return RedirectToAction("AdminIndex");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View(ven);
            }
        }

        // GET: Vendor/Delete/5
        public ActionResult DeleteVen(int id)
        {
            if (ModelState.IsValid)
            {
                object obj = (object)id;
                bool a = _busRepository.deleteven(id);
                if (a)
                {
                    return RedirectToAction("AdminIndex");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        

        public ActionResult DeleteBus(string id)
        {
            object z = (object)(id);

            if (ModelState.IsValid)
            {
                Bus bus = _busRepository.find(z);
                object obj = (object)id;
                bool a = _busRepository.delete(obj);
                if (a)
                {
                    return RedirectToAction("VendorBusses", new { id = bus.vendorid });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

    }
}
