using Busticket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookingId { get; set; }

        [ForeignKey("Passenger")]
        public int PassengerId { get; set; }

        [ForeignKey("Seat")]
        public int SeatId { get; set; }

        //[ForeignKey("Bus")]
        //public string BusNo { get; set; }

        [ForeignKey("BusLog")]
        public int BusLogId {  get; set; }

        public DateTime BookingDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsPaid { get; set; }


        // Navigation properties
        public virtual Passenger Passenger { get; set; }
        public virtual Seat Seat { get; set; }

       

        public virtual BusLog BusLog { get; set; }
    }
}

