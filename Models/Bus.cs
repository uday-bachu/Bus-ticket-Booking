using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class Bus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Bus Number")]
        [RegularExpression("^[A-Z]{2}[0-9]{2}[A-HJ-NP-Z]{1,2}[0-9]{4}$")]
        public string BusNo { get; set; }
        [Required]
        [DisplayName("Total Seats")]
        public int seats { get; set; }

        [Required]
        [ForeignKey("vendor")]
        public int vendorid { get; set; }


        [Required]
        [DisplayName("Driver Name")]
        public string driver { get; set; }


        [DisplayName("Vendor Name")]
        public string VendorName { set { value = vendor.name; } }// Custom property to get vendor name directly

        [Required(ErrorMessage = "Origin is required")]
        public string Origin { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Departure Time is required")]
        public DateTime StartDay { get; set; }

        [Required(ErrorMessage = "Arrival Time is required")]
        public DateTime EndDay { get; set; }



        

        // Navigation properties


        public virtual Vendor vendor { get; set; }

        public virtual ICollection<BusLog> BusLogs { get; set; }

        public virtual ICollection<Seat> GetSeats { get; set; }

        //public virtual ICollection<Booking> Bookings { get; set; }



       


    }
}