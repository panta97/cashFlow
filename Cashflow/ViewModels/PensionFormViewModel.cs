using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cashflow.Models;

namespace Cashflow.ViewModels
{
    public class PensionFormViewModel
    {
        public Flujo Flujo { get; set; }
        public Miembro Miembro { get; set; }
        public IEnumerable<Miembro> Miembros { get; set; }
        public Pension Pension { get; set; }
        public IEnumerable<Pension> Pensiones { get; set; }
    }
}