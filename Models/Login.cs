using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Busticket.Models
{
    public class Login
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoginId { get; set; }

        public string password { get; set; }

        public string Role { get; set; }
    }
}