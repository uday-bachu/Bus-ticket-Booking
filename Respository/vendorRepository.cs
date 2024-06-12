using Busticket.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Busticket.Respository
{
    public class vendorRepository 
    {
        DbBooking context;

        public vendorRepository(DbBooking context)
        {
            this.context = context;
        }

        public bool VendorPassword(int loginId, string passWord)
        {
            Login login = context.Logins.Where(a => a.LoginId == loginId).FirstOrDefault();
            if (login == null)
            {
                context.Logins.Add(new Login
                {
                    LoginId = loginId,
                    password = passWord,
                    Role = "Vendor"
                });

            }
            else if (login != null)
            {
                login.password = passWord;

            }
            return context.SaveChanges() > 0;

        }


        public List<Bus> VendorBusesList(int id) 
        {
            return context.Buses.Where(a=>a.vendorid == id).ToList();
        
        }
        public bool add(Vendor v)
        {
            try
            {
                context.Vendors.Add(v);
                int r = context.SaveChanges();
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool delete(object id)
        {

            return false;
        }

        public Vendor find(object id)
        {
            int Id = (int)id;
            return context.Vendors.Find(Id);
        }

        public List<Vendor> GetAll()
        {
            return context.Vendors.ToList();
        }

        public bool update(object id, Vendor v)
        {
            
                int toupdateid = (int)id;
                Vendor ven = find(id);
                if (ven == null)
                {
                    return false;
                }
                ven.name = v.name;
                ven.age = v.age;
                int r = context.SaveChanges();
                if (r > 0)
                    return true;
                else
                    return false;
            
        }
    }
}