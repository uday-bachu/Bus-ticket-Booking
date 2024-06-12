using Busticket.Models;
using System;
using Busticket.programs;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Busticket.Respository
{
    public class SeatRepository
    {
        DbBooking _context;
        public SeatRepository(DbBooking context)
        {
            this._context = context;
        }






        public bool bookseat(int passengerId,int busLogId, int seatId)
        {
            try
            {
                UniqueRandomNumberGenerator generator = new UniqueRandomNumberGenerator();
                int a = generator.GenerateUniqueRandomNumber();

                var seat = _context.GetSeats.FirstOrDefault(s => s.SeatId == seatId);
                if (seat == null)
                    return false; // Seat not available or already booked

                int bookingId = a; // You need to implement this method

                seat.IsBooked = true;
                _context.Bookings.Add(new Booking { BookingId = bookingId, PassengerId = passengerId, SeatId = seatId, BookingDate = DateTime.Now, BusLogId = busLogId });
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while booking the seat
            }
        }

        public bool markaspaid(int passengerId, int busLogId, int seatId)
        {
            try
            {
                
                Booking booking = _context.Bookings.FirstOrDefault(a => a.PassengerId == passengerId  && a.SeatId == seatId && a.BusLogId==busLogId);
                if (booking == null)
                    return false; // Booking not found

                booking.IsPaid = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while marking as paid
            }
        }


























        public bool BookSeat(int passengerId, string busNo, DateTime date, int seatId)
        {
            try
            {
                UniqueRandomNumberGenerator generator = new UniqueRandomNumberGenerator();
                int a = generator.GenerateUniqueRandomNumber();

                var seat = _context.GetSeats.FirstOrDefault(s => s.SeatId == seatId);
                if (seat == null)
                    return false; // Seat not available or already booked

                int bookingId = a; // You need to implement this method

                seat.IsBooked = true;
                _context.Bookings.Add(new Booking { BookingId = bookingId, PassengerId = passengerId, SeatId = seatId, BookingDate = DateTime.Now, BusLogId = seat.BusLogId });
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while booking the seat
            }
        }

        public bool MarkAsPaid(int passengerId, string busNo, DateTime date, int seatId)
        {
            try
            {
                var booking = _context.Bookings.FirstOrDefault(a => a.PassengerId == passengerId && a.Seat.BusLog.BusNo == busNo && a.SeatId == seatId && a.Seat.BusLog.DepartureTime.Date == date);
                if (booking == null)
                    return false; // Booking not found

                booking.IsPaid = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while marking as paid
            }
        }


        public List<int> BuslogSeatAvailable(string busno)
        {
            List<BusLog> busLogs = busLogList(busno);
            List<int> seats = new List<int>();

            foreach (var busLog in busLogs)
            {
                int availableSeats = _context.GetSeats
                    .Where(x => x.BusLogId == busLog.Id && !x.IsBooked)
                    .Count();
                seats.Add(availableSeats);
            }

            return seats;
        }


        public List<BusLog> busLogList(string busno)
        {
            return _context.BusLogs.Where(a => a.BusNo == busno).ToList();
        } 




    }

}



/*
 public bool BookSeat(int passengerId,string busNo, int seatId)
        {
            Seat seat = _context.GetSeats.Where(s => s.BusLog.BusNo == busNo && s.SeatId == seatId).FirstOrDefault();
            UniqueRandomNumberGenerator generator = new UniqueRandomNumberGenerator();
            int a = generator.GenerateUniqueRandomNumber();

            try
            {
                seat.IsBooked = true;
                _context.Bookings.Add(new Booking {BookingId =a,PassengerId = passengerId, SeatId = seatId, BookingDate = DateTime.Now, BusLogId = seat.BusLogId });
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while booking the seat
            }
        }


        
        public List<Seat> SeatsAvailable(string busno)
        {
            return _context.GetSeats.Include("Bus").Where(b => b.BusLog.BusNo == busno && b.IsBooked == false).ToList();
        }

        public IEnumerable<Seat> GetSeatsForBus(string busno)
        {
            return _context.GetSeats.Where(s => s.BusLog.BusNo == busno).ToList();
        }


        public bool MarkAsPaid(int passengerId, string busNo, int seatId)
        {
            Booking passenger = _context.Bookings.Where(a => a.PassengerId == passengerId && a.Seat.BusLog.BusNo == busNo && a.SeatId == seatId).FirstOrDefault();
            if (passenger == null)
            {
                return false; // Passenger not found
            }
            try
            {
                passenger.IsPaid = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Error occurred while marking as paid
            }
        }
 */