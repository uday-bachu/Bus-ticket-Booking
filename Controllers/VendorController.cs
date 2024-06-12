using Busticket.Models;
using Busticket.Respository;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Busticket.Controllers
{
    public class VendorController : Controller
    {
        private readonly BusRepository _busRepository;
        private readonly vendorRepository _vendorRepository; // Add a reference to VendorRepository
        private readonly SeatRepository _seatRepository;


        private readonly PassengerReepository _passengerRepository;

        public VendorController()
        {
            _busRepository = new BusRepository(new DbBooking());
            _seatRepository = new SeatRepository(new DbBooking());
            _vendorRepository = new vendorRepository(new DbBooking()); // Initialize VendorRepository
            _passengerRepository = new PassengerReepository(new DbBooking());

        }

       
        public ActionResult VendorIndex()
        {
            int id = (int)Session["Id"];
            object obj = (object)id;
            Vendor ven = _busRepository.VendeDetails(obj);
            return View(ven);
        }

        public ActionResult VenDetails()
        {
            int id = (int)Session["Id"];
            Vendor ven = _busRepository.VendeDetails(id);
            return View(ven);
        }

        public ActionResult AllPassengers() //Passengers in Vendor Shared View
        {
            int venId = (int)Session["Id"];
            List<Booking> passengers = _busRepository.VenPassengers(venId);
            return View(passengers);
        }
        /*
        public ActionResult VendorHome() //Home- in Vendor Shared View
        {
            int a = (int)Session["Id"];
            vendor ven = _busRepository.VendeDetails(a);
            return View(ven);

        }
        */
        public ActionResult BusPassengers(int id)
        {
            var passengers = _busRepository.passengerbookings(id);
            return View(passengers);
        }


        public ActionResult Allbuses()
        {
            int vendorId = (int)Session["Id"];
            List<Bus> buses = _busRepository.VendorBusesList(vendorId);

            return View(buses);
        }


        public ActionResult VendorBusses()
        {
            int id = (int)Session["Id"];
            List<Bus> buses = _busRepository.VendorBusesList(id);
            return View(buses);
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
        public ActionResult EditVen(int id, Vendor ven)
        {
            if (ModelState.IsValid)
            {
                object a = (object)id;
                bool b = _vendorRepository.update(a, ven);
                if (b)
                {
                    return RedirectToAction("VendorIndex",new {id = ven.id});
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


        public ActionResult EditBus(string id)
        {
            object obj = (object)id;
            Bus bu = _busRepository.find(obj);
            ViewBag.StartDay = bu.StartDay;
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
                    return RedirectToAction("VendorBusses", new { id = bus.vendorid });
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


        // GET: Bus/Create
        public ActionResult CreateBus()
        {
            Bus bus = new Bus();    
            bus.vendorid = (int)Session["Id"];
            
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
                    return RedirectToAction("VendorBusses", new {id = bus.vendorid});
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

       
        public ActionResult BusDetails(string id)
        {
            object a = (object)id;

            Bus bus = _busRepository.find(a);

            List<BusLog> busLogs = _seatRepository.busLogList(id);
            ViewBag.JourneyDays = busLogs.Count();


            return View(bus);
        }


        public ActionResult BusLogs(string busNo)
        {
            

            List<BusLog> busLogs = _seatRepository.busLogList(busNo);
            List<int> busLogseats = _seatRepository.BuslogSeatAvailable(busNo);
            ViewBag.seats = busLogseats;
            return View(busLogs);
        }


        public ActionResult DeleteBusLog(int id)
        {
            BusLog busLog = _busRepository.findBusLog(id);

            bool a = _busRepository.deletebusLog(id);
            
            return RedirectToAction("BusLogs", new { busNo = busLog.BusNo });
           

        }


        public ActionResult DeleteBus(string id)
        {
            object z = (object)(id);

            if (ModelState.IsValid)
            {
                Bus bus = _busRepository.find(z);
                
                bool a = _busRepository.delete(z);
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

