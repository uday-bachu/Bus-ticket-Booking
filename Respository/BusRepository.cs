using Busticket.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Busticket.Respository
{
    public class BusRepository 
    {
        DbBooking context ;
        public BusRepository(DbBooking context) 
        {
            this.context = context;
        }


        public BusLog BusSelectionMethod (string origin, string destination, DateTime journeyDate)
        {
            Bus bus = context.Buses
                .Where(a => a.Origin.Equals(origin, StringComparison.OrdinalIgnoreCase)
                         && a.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();


            if (bus != null)
            {
                BusLog busLog = context.BusLogs.Where(a => a.BusNo.Equals(bus.BusNo, StringComparison.OrdinalIgnoreCase) && a.ArrivalTime.Day == journeyDate.Day).FirstOrDefault();

                if (busLog != null)
                {
                    return busLog;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        public List<Seat> getavailableseats(int busLogId)
        {
            return context.GetSeats.Where(s => s.BusLogId == busLogId && !s.IsBooked).ToList();
        }


        public List<Bus> GetAll()
        {
            return context.Buses.ToList();
        }


        public Vendor VendeDetails(object id)
        {
            int venID = (int)id;
            return context.Vendors.Find(venID);
        }


        public List<Bus> VendorBusesList(int id)
        {
            return context.Buses.Where(a => a.vendorid == id).ToList();

        }


        public List<Booking> passengerbookings(int id)
        {
            List<Booking> bookings = context.Bookings.Where(a => a.BusLogId == id).ToList();
            return bookings;
        }




        public int TotalSeatAvailable(string busno)
        {
            return context.GetSeats.Count(s => s.BusLog.BusNo == busno && !s.IsBooked);
        }

        public List<Booking> Allpassengerbookings(string id)
        {
            List<Booking> bookings = context.Bookings.Where(a => a.BusLog.BusNo == id).ToList();
            return bookings;
        }



        public List<Booking> VenPassengers(int venId)
        {
            List<Bus> busses = context.Buses.Where(a => a.vendorid == venId).ToList();
            List<Booking> passengers = new List<Booking>();
            foreach (var bus in busses)
            {
                List<Booking> passengeronBus = Allpassengerbookings(bus.BusNo);
                passengers.AddRange(passengeronBus);
            }
            return (passengers);
        }



       

        public bool add(Bus entity)
        {
            context.Buses.Add(entity);

            int days = (entity.EndDay - entity.StartDay).Days;

            for (int i = 0; i <= days; i++)
            {
                DateTime departureDate = entity.StartDay.AddDays(i);
                DateTime arrivalTime = departureDate.Date + entity.EndDay.TimeOfDay;

                
                var busLog = new BusLog { BusNo = entity.BusNo,JourneyDay =i+1, DepartureTime = departureDate, ArrivalTime = arrivalTime, IsAvailable = true };
                context.BusLogs.Add(busLog);

                context.SaveChanges();

                for (int j = 1; j <= entity.seats; j++)
                {
                    context.GetSeats.Add(new Seat { SeatName = j, BusLogId = busLog.Id });
                }
            }

            return context.SaveChanges() > 0;
        }




        public List<DateTime> GetAvailableDates(string busNo)
        {
            return context.BusLogs
                          .Where(bl => bl.BusNo == busNo && bl.IsAvailable)
                          .Select(bl => bl.DepartureTime)
                          
                          .ToList();
        }


        public List<Seat> GetAvailableSeats(string busNo, DateTime date)
        {
            return context.GetSeats
                          .Where(s => s.BusLog.BusNo == busNo && s.BusLog.DepartureTime == date && !s.IsBooked)
                          .ToList();
        }






        public bool delete(object id)
        {
            string toDeleteId = (string)id;
            Bus toDelete = context.Buses.Find(toDeleteId);

            if (toDelete != null)
            {
                List<Booking> bookings = context.Bookings.Where(b => b.BusLog.BusNo == toDeleteId).ToList();
                foreach (var booking in bookings)
                {
                    context.Bookings.Remove(booking);
                }

                List<BusLog> busLogs = context.BusLogs.Where(bl => bl.BusNo == toDeleteId).ToList();
                foreach (var busLog in busLogs)
                {
                    List<Seat> seats = context.GetSeats.Where(s => s.BusLogId == busLog.Id).ToList();
                    context.GetSeats.RemoveRange(seats);
                }
                context.BusLogs.RemoveRange(busLogs);
                context.Buses.Remove(toDelete);

                return context.SaveChanges() > 0;
            }
            return false;
        }


        public bool deleteven(int venId)
        {
            
            List<Bus> busses = context.Buses.Where(a => a.vendorid == venId).ToList();
            Models.Login login = context.Logins.Where(a =>a.LoginId == venId).FirstOrDefault();
            Vendor todelete = context.Vendors.Find(venId);
            int r = 0;
            if (todelete != null)
            {

                foreach(var  bus in busses)
                {
                    object a = (object)bus.BusNo;
                    delete(a);
                }

                context.Vendors.Remove(todelete);
                context.Logins.Remove(login);
                r = context.SaveChanges();
            }
            return r > 0;
            }

        public Bus find(object id)
        {
            string BusId = (string)id;
            return context.Buses.Find(BusId);
        }


        public BusLog findBusLog(int id)
        {
            return context.BusLogs.Find(id);
        }


        public bool deletebusLog(int id)
        {
            BusLog buslog = context.BusLogs.Where(a => a.Id == id).FirstOrDefault();
            List<Booking> bookings = context.Bookings.Where(a =>a.BusLogId == id).ToList();
            List<Seat> seats = context.GetSeats.Where(a => a.BusLogId == id).ToList();
            if(buslog != null)
            {
                context.BusLogs.Remove(buslog);
                context.Bookings.RemoveRange(bookings);
                context.GetSeats.RemoveRange(seats);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        
        public bool UpdateBus(string id, Bus entity)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    int originalSeatCount = context.GetSeats.Count(s => s.BusLog.BusNo == id);
                    int originalBusLogCount = context.BusLogs.Count(bl => bl.BusNo == id);

                    // Update the Bus entity
                    var existingEntity = context.Buses.Find(id);
                    if (existingEntity == null)
                        return false;

                    
                    // Calculate the new journey days
                    int updatedJourneyDays = (entity.EndDay - entity.StartDay).Days + 1;
                    int journeyDaysDifference = updatedJourneyDays - originalBusLogCount;


                    existingEntity.seats = entity.seats;
                    existingEntity.driver = entity.driver;
                    existingEntity.Origin = entity.Origin;
                    existingEntity.Destination = entity.Destination;
                    existingEntity.EndDay = entity.EndDay.Date + existingEntity.EndDay.TimeOfDay;

                    // Update bus logs if the number of days has changed
                    if (journeyDaysDifference != 0)
                    {
                        
                        // Add or remove bus logs based on the new journey days
                        if (journeyDaysDifference > 0)
                        {
                            // Add new bus logs
                            for (int i = originalBusLogCount; i <= updatedJourneyDays; i++)
                            {
                                DateTime arrival = entity.StartDay.AddDays(i).Date.Add(existingEntity.EndDay.TimeOfDay);
                                var newBusLog = new BusLog
                                {
                                    BusNo = id,
                                    JourneyDay = i,
                                    DepartureTime = existingEntity.StartDay.AddDays(i),
                                    ArrivalTime = arrival,
                                    IsAvailable = true
                                };
                                context.BusLogs.Add(newBusLog);
                                context.SaveChanges(); // Save to get the BusLog Id

                                UpdateSeat(newBusLog.Id, entity);
                            }
                        }
                        else if (journeyDaysDifference < 0)
                        {
                            // Remove excess bus logs
                            var excessBusLogs = context.BusLogs.Where(bl => bl.BusNo == id)
                                                               .OrderByDescending(bl => bl.DepartureTime)
                                                               .Take(-journeyDaysDifference-1)
                                                               .ToList();

                            // Remove associated seats for removed bus logs
                            foreach (var busLog in excessBusLogs)
                            {
                                var excessSeats = context.GetSeats.Where(s => s.BusLogId == busLog.Id).ToList();
                                var excessBookings = context.Bookings.Where(a => a.BusLogId == busLog.Id).ToList();
                                context.Bookings.RemoveRange(excessBookings);
                                context.GetSeats.RemoveRange(excessSeats);
                            }

                            context.BusLogs.RemoveRange(excessBusLogs);
                        }
                    }

                    int OriginalSeatCount = existingEntity.seats;
                    int UpdatedSeatCount = entity.seats;
                    int diff = OriginalSeatCount - UpdatedSeatCount;
                    if (diff != 0)
                    {
                        var busLogs = context.BusLogs.Where(bl => bl.BusNo == id).ToList();
                        foreach (var busLog in busLogs)
                        {
                            UpdateSeat(busLog.Id, entity);
                        }
                    }
                    // Update seats for existing bus logs




                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool UpdateSeat(int busLogId, Bus entity)
        {
            try
            {
                int originalSeatCount = context.GetSeats.Count(s => s.BusLogId == busLogId);

                // Add new seats if needed
                if (originalSeatCount < entity.seats)
                {
                    for (int i = originalSeatCount + 1; i <= entity.seats; i++)
                    {
                        context.GetSeats.Add(new Seat { SeatName = i, BusLogId = busLogId });
                    }
                }
                // Remove excess seats if needed
                else if (originalSeatCount > entity.seats)
                {
                    var excessSeats = context.GetSeats.Where(s => s.BusLogId == busLogId)
                                                      .OrderByDescending(s => s.SeatName)
                                                      .Take(originalSeatCount - entity.seats)
                                                      .ToList();

                    foreach (var seat in excessSeats)
                    {
                        var booking = context.Bookings.FirstOrDefault(b => b.SeatId == seat.SeatId);
                        if (booking != null)
                        {
                            context.Bookings.Remove(booking);
                        }
                        context.GetSeats.Remove(seat);
                    }
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}





/*


       public List<(Passenger passenger, int seatName)> GetPassengersByBusNo(string busNo)
       {
           var bookings = context.Bookings.Where(b => b.Seat.BusNo == busNo).ToList();

           var passengersWithSeats = new List<(Passenger, int)>();

           foreach (var booking in bookings)
           {
               var passenger = context.Passengers.Find(booking.PassengerId);
               if (passenger != null)
               {
                   passengersWithSeats.Add((passenger, booking.Seat.SeatName));
               }
           }

           return passengersWithSeats;
       }



       public Passenger getpassenger(string Busno,int passengerid)
       {
           List<Booking> bookings = context.Bookings.Where(a => a.Seat.BusNo == Busno).ToList();

           List<Passenger> passengers = new List<Passenger>();

           foreach (Booking booking in bookings)
           {
               var passenger = context.Passengers.Find(booking.PassengerId);
               if (passenger != null)
               {
                   passengers.Add(passenger);
               }
           }
           return passengers.Where( a=> a.PassengerId == passengerid).FirstOrDefault();
       }


       public List<Vendor> GetVendors()
       {
           return context.Vendors.ToList();
       }



       public int AvailableSeats(string busno)
       {
           List<Seat> a = context.GetSeats.Where(b => b.BusNo == busno & b.IsBooked == false).ToList();
           int c = a.Count();
           return c;

       }
       */

//public bool add(Bus entity)
//{
//    context.Buses.Add(entity);

//    int days = entity.EndDay.Day - entity.StartDay.Day;
//    for (int i = 0; i <= days; i++)
//    {

//        DateTime departureDate = entity.StartDay.AddDays(i);

//        DateTime arrivalTime = departureDate.Date + entity.EndDay.TimeOfDay;

//        context.BusLogs.Add(new BusLog { JourneyDay = i + 1, BusNo = entity.BusNo, DepartureTime = departureDate, ArrivalTime = arrivalTime, IsAvailable = true });

//        for (int j = 1; j <= entity.seats; j++)
//        {
//            context.GetSeats.Add(new Seat { SeatName = j , BusLogId= });
//        }

//    }

//    return context.SaveChanges() > 0;
//}



//public bool UpdateBus(string id, Bus entity)
//{
//    using (var transaction = context.Database.BeginTransaction())
//    {
//        try
//        {
//            int originalSeatCount = context.GetSeats.Count(s => s.BusNo == id);

//            // Update the Bus entity
//            var existingEntity = context.Buses.Find(id);
//            if (existingEntity == null)
//                return false;

//            existingEntity.seats = entity.seats;
//            existingEntity.driver = entity.driver;

//            // Update seats
//            if (originalSeatCount < entity.seats)
//            {
//                // Add new seats
//                for (int i = originalSeatCount + 1; i <= entity.seats; i++)
//                {
//                    context.GetSeats.Add(new Seat { SeatName = i, BusNo = id });
//                }
//            }
//            else if (originalSeatCount > entity.seats)
//            {
//                // Remove excess seats
//                var excessSeats = context.GetSeats.Where(s => s.BusNo == id)
//                                                  .OrderByDescending(s => s.SeatName)
//                                                  .Take(originalSeatCount - entity.seats);
//                context.GetSeats.RemoveRange(excessSeats);
//            }

//            context.SaveChanges();
//            transaction.Commit();
//            return true;
//        }
//        catch (Exception)
//        {
//            transaction.Rollback();
//            throw;
//        }
//    }
//}


//public bool delete(object id )
//{
//    string todeleteid = (string)id;

//    Bus todelete = context.Buses.Find(todeleteid);

//    List<Booking> bookings = context.Bookings.Where(a => a.BusLog.Bus.BusNo == todeleteid).ToList();

//    List<Seat> seats = context.GetSeats.Where(a => a.BusNo ==todeleteid).ToList();



//        int r = 0;
//        if(todelete != null)
//        {
//            context.Buses.Remove(todelete);
//            context.Bookings.RemoveRange(bookings);
//            context.GetSeats.RemoveRange(seats);

//            r = context.SaveChanges();
//        }
//        if (r > 0)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }

//}



//public List<Passenger> buspassengers(string Busno)
//{
//    List<Booking> bookings = context.Bookings.Where(a => a.Seat.BusNo == Busno).ToList();

//    List<Passenger> passengers = new List<Passenger>();

//    foreach (Booking booking in bookings)
//    {
//        var passenger = context.Passengers.Find(booking.PassengerId);
//        if(passenger != null)
//        {
//            passengers.Add(passenger);
//        }
//    }
//    return passengers;

//}