using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cashflow.Dtos
{
    public class PensionEditDto
    {
        public byte PensionId { get; set; }
        public decimal Aporte { get; set; }
    }
}