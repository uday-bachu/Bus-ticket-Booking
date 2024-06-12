using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class BusLog
    {
        [Key]
        public int Id { get; set; }

        public int JourneyDay { get; set; }

        [ForeignKey("Bus")]
        public string BusNo { get; set; }


        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }



        [DefaultValue(true)]
        public bool IsAvailable { get; set; }

        public virtual Bus Bus { get; set; }

        public virtual ICollection<Seat> GetSeats{ get; set;}

       

    }
}