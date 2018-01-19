using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cashflow.ViewModels
{
    class Pago
    {
        public decimal Remuneracion { get; set; }
        public bool Gratificaciones { get; set; }
    }

    public class PagoFormViewModel
    {
        public decimal Remuneracion { get; set; }
        public double Seguro { get; set; }
        public bool Gratificaciones { get; set; }
    }

}