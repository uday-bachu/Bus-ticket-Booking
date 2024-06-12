using Busticket.Models;
using Busticket.programs;
using Busticket.Respository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Busticket.Controllers
{
    public class PassengerController : Controller
    {
        private readonly BusRepository _busRepository;
        private readonly PassengerReepository _passengerRepository;
        private readonly SeatRepository _seatRepository;
        
        public PassengerController()
        {
            _busRepository = new BusRepository(new DbBooking());
            _passengerRepository= new PassengerReepository(new DbBooking());
            _seatRepository = new SeatRepository(new DbBooking());
            
        }

        

        // GET: Passenger/Details/5
        

        // GET: Passenger/Create
        public ActionResult Create()
        {
            Passenger passenger = new Passenger();
            UniqueRandomNumberGenerator generator = new UniqueRandomNumberGenerator();
            passenger.PassengerId = generator.GenerateUniqueRandomNumber();
            return View(passenger);
        }

        // POST: Passenger/Create
        [HttpPost]
        public ActionResult Create(int passengerId,Passenger passenger)
        {
            if(ModelState.IsValid)
            {
                bool a =_passengerRepository.add(passenger);
                if (a)
                {
                    return RedirectToAction("PasssengerPassword", new { passengerId = passenger.PassengerId });
                }
                else
                {
                    return View("Error");
                }
                
            }
           
                // TODO: Add insert logic here
            else
            { return View(passenger); }
                
            
        }

        public ActionResult PasssengerPassword(int passengerId)
        {
            ViewBag.PassengerId = passengerId; // Passing passengerId to the view
            return View();
        }

        [HttpPost]
        public ActionResult PasssengerPassword(int passengerId, string password1, string password2)
        {
           
            if (password1 == null || password2 == null)
                return View("Error");

            if (password1 != password2)
                return Content("Provided passwords do not match");

            bool passwordChanged = _passengerRepository.passengerPassword(passengerId, password1);

            if (passwordChanged)
            {
                Session.Add("Id", passengerId);
                return RedirectToAction("PassengerIndex");
            }
            else
            {
                return View("Error");
            }
        }
        
        public ActionResult PassengerHome() //passengerdetails
        {
            int passengerId = (int)Session["Id"];
            Passenger passenger = _passengerRepository.GetPassenger(passengerId);
            return View(passenger);

        }

        public ActionResult Details() //alias passengerhome
        {
            int passengerId =(int)Session["Id"];
            Passenger passenger = _passengerRepository.GetPassenger(passengerId);
            return View(passenger);
        }

        public ActionResult passengerBooking()
        {
            int passengerId = (int)Session["Id"];
            var bookings = _passengerRepository.PassBookings(passengerId);
            return View(bookings);
            
        }

        public ActionResult PassengerIndex()
        {
            int a = (int)Session["Id"];
            Passenger passenger = _passengerRepository.GetPassenger(a);
            return View(passenger);
        }


        public ActionResult SelectBus()
        {
            int a = (int)Session["Id"];
            ViewBag.PassengerId = a;
            return View();
        }
        [HttpPost]
        public ActionResult SelectBus(FormCollection frmc)
        {
            string origin = frmc["Origin"];
            string destination = frmc["Destination"];
            string journeydatestring = frmc["selectedDate"];
            int a = (int)Session["Id"];
            DateTime journeyDate = DateTime.Parse(journeydatestring).Date + DateTime.Now.TimeOfDay ;
            BusLog busLog = _busRepository.BusSelectionMethod(origin, destination, journeyDate);
            if (busLog != null)
            {
                return RedirectToAction("SelectSeat", new { passengerId = a, seatLogId  = busLog.Id});
            }
            else
            {
                // Handle booking failure
                ViewBag.ErrorMessage = "No busses Matching the inputs";
                return View();
            }


        }


        public ActionResult SelectSeat(int passengerId,int seatLogId)
        {
            // Get available seats for the selected bus on the selected date
            var availableSeats = _busRepository.getavailableseats(seatLogId);

            // Pass available seats to the view
            ViewBag.PassengerId = passengerId;
            ViewBag.SeatLogId = seatLogId;

            //ViewBag.SelectedDate = selectedDate;
            return View(availableSeats);
        }

        [HttpPost]
        public ActionResult SelectSeat(int passengerId, int seatLogId, int seatId)
        {
            // Book the selected seat for the passenger 
            string pay = Request.Form["paymentStatus"];
            bool payment = pay.Equals("Paid");

            bool res = _seatRepository.bookseat(passengerId, seatLogId, seatId);

            if (res && payment)
            {
                _seatRepository.markaspaid(passengerId, seatLogId, seatId);
                return RedirectToAction("Details");
            }
            else if (res && !payment)
            {
                return RedirectToAction("Details");
            }
            else
            {
                // Handle booking failure
                ViewBag.ErrorMessage = "Failed to book the seat.";
                return View();
            }
        }





        // GET: Passenger/Edit/5
        public ActionResult Edit()
        {
            int id = (int)Session["Id"];
            Passenger pass = _passengerRepository.GetPassenger(id);
           
            return View(pass);
        }

        // POST: Passenger/Edit/5
        [HttpPost]
        public ActionResult Edit(Passenger passenger)
        {
            int id = (int)Session["Id"];
            
                if(ModelState.IsValid) 
                {
                    if(_passengerRepository.edit(id, passenger))
                    {
                        return RedirectToAction("PassengerIndex");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                return View(passenger);
            
        }
        

      
        // GET: Passenger/Delete/5
        public ActionResult Delete(int id)
        {
            
            _passengerRepository.Delete(id);
           
            return RedirectToAction("Index","Home");
        }

        public ActionResult DelBooking(int bookingId)
        {
            _passengerRepository.delBooking(bookingId);
            return RedirectToAction("passengerBooking");
        }

        public ActionResult Contact()
        {
            return View();
        }

        //public async Task<ActionResult> Create(Passenger passenger)


    }

}





/*
       public ActionResult SelectBus(int passengerId)
       {

           var buses = _busRepository.GetAll();
           ViewBag.PassengerId = passengerId;
           return View(buses);
       }

       [HttpPost]
       public ActionResult SelectBus(int passengerId, string busNo)
       {
           return RedirectToAction("SelectSeat", new { passengerId = passengerId, busNo = busNo });
       }

       // GET: Passenger/SelectSeat/{passengerId}/{busNo}
       public ActionResult SelectSeat(int passengerId, string busNo)
       {
           var availableSeats = _seatRepository.SeatsAvailable(busNo);

           Session.Add("BusNo", busNo);

           return View(availableSeats);
       }

       [HttpPost]
       public ActionResult SelectSeat(FormCollection frmc)
       {
           // Book the selected seat for the passenger 
           int passengerId = (int)Session["Id"];
           string busNo = (string)Session["BusNo"];
           int seatId = Convert.ToInt32(frmc["seatId"]);
           string pay = frmc["paymentStatus"];
           bool payment = false;
           if (pay == "Paid")
           {
               payment = true;
           }



           bool res = _seatRepository.BookSeat(passengerId, busNo, seatId);
           if (res && payment)
           {

               _seatRepository.MarkAsPaid(passengerId, busNo, seatId);
               return RedirectToAction("Details", new { PassengerId = passengerId });
           }
           else
           {
               // Handle booking failure
               ViewBag.ErrorMessage = "Failed to book the seat.";
               return Content("Failed");
           }
       }



        

       
        public ActionResult SelectBus(int passengerId)
        {
            var buses = _busRepository.GetAll();
            ViewBag.PassengerId = passengerId;
            return View(buses);
        }

        [HttpPost]
        public ActionResult SelectBus(int passengerId, string busNo)
        {
            Session.Add("PassengerId", passengerId);
            Session.Add("BusNo", busNo);
            return RedirectToAction("SelectDate", new { passengerId = passengerId, busNo = busNo });
        }

        public ActionResult SelectDate(int passengerId, string busNo)
        {
            // Get available dates for the selected bus
            var availableDates = _busRepository.GetAvailableDates(busNo);

            // Pass available dates to the view
            ViewBag.PassengerId = passengerId;
            ViewBag.BusNo = busNo;
            return View(availableDates);
        }

        [HttpPost]
        public ActionResult SelectDate(int passengerId, string busNo, DateTime selectedDate)
        {
            Session.Add("SelectedDate", selectedDate);
            return RedirectToAction("SelectSeat", new { passengerId = passengerId, busNo = busNo, selectedDate = selectedDate });
        }

        // GET: Passenger/SelectSeat/{passengerId}/{busNo}
        public ActionResult SelectSeat(int passengerId, string busNo, DateTime selectedDate)
        {
            // Get available seats for the selected bus on the selected date
            var availableSeats = _busRepository.GetAvailableSeats(busNo, selectedDate);

            // Pass available seats to the view
            ViewBag.PassengerId = passengerId;
            ViewBag.BusNo = busNo;
            ViewBag.SelectedDate = selectedDate;
            return View(availableSeats);
        }

        [HttpPost]
        public ActionResult SelectSeat(int passengerId, string busNo, DateTime selectedDate, int seatId)
        {
            // Book the selected seat for the passenger 
            string pay = Request.Form["paymentStatus"];
            bool payment = pay .Equals("Paid");

            bool res = _seatRepository.BookSeat(passengerId, busNo, selectedDate, seatId);

            if (res && payment)
            {
                _seatRepository.MarkAsPaid(passengerId, busNo, selectedDate, seatId);
                return RedirectToAction("Details");
            }
            else
            {
                // Handle booking failure
                ViewBag.ErrorMessage = "Failed to book the seat.";
                return View();
            }
        }




       */