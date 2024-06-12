using Busticket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;


namespace Busticket.Respository
{
    public class PassengerReepository
    {
        DbBooking _context;
        public PassengerReepository(DbBooking _context)
        {
            this._context = _context;
            
        }
        
        
        public bool passengerPassword(int loginId,string passWord)
        {
            Login login = _context.Logins.Where(a => a.LoginId == loginId).FirstOrDefault();
            if (login == null)
            {
                _context.Logins.Add(new Login
                {
                    LoginId = loginId,
                    password = passWord,
                    Role = "Passenger"
                });
                
            }
            else if (login !=null)
            {
                login.password = passWord;
               
            }
            return _context.SaveChanges() > 0;

        }

       

        public List<Passenger> GetAll()
        {
            return _context.Passengers.ToList();
        }

        public Passenger GetPassenger(int passengerId)
        {
            return _context.Passengers.Find(passengerId);
        }


        public List<(Booking booking, int seatName)> GetBookingDetailsByPassengerId(int passengerId)
        {
            var bookingDetails = (from booking in _context.Bookings
                                  where booking.PassengerId == passengerId
                                  join seat in _context.GetSeats on booking.SeatId equals seat.SeatId
                                  select new { booking, seatName = seat.SeatName }).ToList();

            var result = bookingDetails.Select(b => (b.booking, b.seatName)).ToList();

            return result;
        }


        public bool add(Passenger passenger)
        {
           
            _context.Passengers.Add(passenger);
            return _context.SaveChanges() > 0;
        }
        
       
         
        public bool edit(int id,Passenger passenger)
        {
            var existingEntity = _context.Passengers.Find(id);
            if(existingEntity == null)
            {
                return false;
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(passenger);
            return _context.SaveChanges() > 0;
        }
        
         
         
         

        public bool Delete(int passengerId)
        {
            Passenger passenger = _context.Passengers.Find(passengerId);
            List<Booking> bookings= _context.Bookings.Where(a =>a.PassengerId == passengerId).ToList();
            Login login = _context.Logins.Where(a =>a.LoginId == passengerId).FirstOrDefault();
            
            if (passenger != null) 
            {
                foreach (var b in bookings)
                {
                    b.Seat.IsBooked = false;
                }

                _context.Passengers.Remove(passenger);
                _context.Bookings.RemoveRange(bookings);
                _context.Logins.Remove(login);
                
                return _context.SaveChanges() > 0;
            }
            else
            { return false; }
            
        }
        
        public bool delBooking(int bookingId)
        {
            Booking booking = _context.Bookings.Find(bookingId);
            booking.Seat.IsBooked=false;
            _context.Bookings.Remove(booking);
            return _context.SaveChanges() > 0;
        }


        public List<Booking> PassBookings(int passengerId)
        {
            List<Booking> bookings = _context.Bookings.Where(a => a.PassengerId==passengerId).ToList();
            return bookings;
        }

    }
}

