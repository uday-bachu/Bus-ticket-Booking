using Busticket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Busticket.Respository
{
    public class RouteRepositiry : IRepository<BusRoute>
    {
        DbBooking context;
       

        public BusRoute FindByBusNo(int  busNo)
        {
            return context.BusRoutes.FirstOrDefault(r=>r.BusNo == busNo);
        }

        public bool add(BusRoute entity)
        {
            context.BusRoutes.Add(entity);   
            int r = context.SaveChanges();
            return r > 0;
        }

        public bool delete(object id)
        {
            int a = (int)id;
            
            BusRoute entity = FindByBusNo(a);
            int r = 0;
            if (entity != null)
            {
                context.BusRoutes.Remove(entity);
                r = context.SaveChanges();
                
            }
            return r > 0;
        }

        public BusRoute find(object id)
        {
            int a = (int)id;
            return FindByBusNo(a);
            
        }

        public List<BusRoute> GetAll()
        {
            return context.BusRoutes.ToList();
        }

        public bool update(object id, BusRoute entity)
        {
            int busno = (int)id;
            BusRoute busRoute = FindByBusNo(busno);
            int r = 0;
            if(busRoute != null)
            {
                busRoute.ArrivalTime = entity.ArrivalTime;
                busRoute.Origin = entity.Origin;
                busRoute.DepartureTime = entity.DepartureTime;
                busRoute.Destination = entity.Destination;
                context.BusRoutes.Add(busRoute);
                r=context.SaveChanges();


            }
            return r>0;
        }
    }
}