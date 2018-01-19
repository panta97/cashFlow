using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cashflow.Dtos
{
    public class DetalleMiembroDto
    {
        public List<decimal> Ingresos { get; set; }
        public List<decimal> Gastos { get; set; }
        public List<string> Miembros { get; set; }
        public List<string> Colors { get; set; }
    }
}