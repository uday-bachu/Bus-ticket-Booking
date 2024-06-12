using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Busticket.Models
{
    public class DbBooking : DbContext
    {
        public DbBooking() : base("sqlcon") { }

        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Seat> GetSeats { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }



        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Bus> Buses { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Login> Logins { get; set; }

        public virtual DbSet<BusLog> BusLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
                .HasRequired(s => s.BusLog)
                .WithMany()
                .HasForeignKey(s => s.BusLogId)
                .WillCascadeOnDelete(false); // Disable cascade delete
        }




    }
}