using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cashflow.Models;

namespace Cashflow.ViewModels
{
    public class IngresoFormViewModel
    {
        public Flujo Flujo { get; set; }

        public IEnumerable<Periodo> Periodos { get; set; }

        public Miembro Miembro { get; set; }
        public IEnumerable<Miembro> Miembros { get; set; }
    }
}