﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cashflow.Models;

namespace Cashflow.Dtos
{
    public class PensionDto
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public byte TipoId { get; set; }

        public string MiembroNombre { get; set; }

        public string PensionNombre { get; set; }
    }
}