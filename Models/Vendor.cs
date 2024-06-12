using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class Vendor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Vendor Id")]
        public int id { get; set; }


        [Required]
        [DisplayName("Vendor Name")]
        public string name { get; set; }



        [EmailAddress(ErrorMessage = "ENter thr valid Mail Adress")]
        public string Mail { get; set; }



        [Phone(ErrorMessage = "Enter Valid Phone number")]
        public string PhoneNo { get; set; }


        [DisplayName("Vendor Age")]
        public int age { get; set; }

        // Navigation properties

        public virtual ICollection<Bus> Buses { get; set; }
    }
}