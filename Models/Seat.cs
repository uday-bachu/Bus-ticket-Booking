using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class Seat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeatId { get; set; }

        [Required]
        public int SeatName { get; set; }

        //[ForeignKey("Bus")]
        //public string BusNo { get; set; }

        [ForeignKey("BusLog")]
        
        
        public int BusLogId { get; set; }


        [DefaultValue(false)]
        public bool IsBooked { get; set; }


        // Navigation property
        public BusLog BusLog { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}