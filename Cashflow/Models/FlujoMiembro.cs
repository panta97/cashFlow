using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cashflow.Models
{
    [Table("FlujoMiembros")]
    public class FlujoMiembro
    {
        public int Id { get; set; }

        public int MiembroId { get; set; }

        public Miembro Miembro { get; set; }

        public int FlujoId { get; set; }

        public Flujo Flujo { get; set; }
    }
}