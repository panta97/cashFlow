using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cashflow.Models
{
    [Table("FlujoPensiones")]
    public class FlujoPension
    {
        public int Id { get; set; }
        public int FlujoId { get; set; }
        public Flujo Flujo { get; set; }
        public byte PensionId { get; set; }
        public Pension Pension { get; set; }
   
    }
}