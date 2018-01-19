using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Cashflow.Dtos;

namespace Cashflow.Models
{
    [Table("Flujos")]
    public class Flujo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo monto es obligatorio")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo fecha es obligatorio")]
        public DateTime Fecha { get; set; }

        [Required]
        public byte TipoId { get; set; }

        public Tipo Tipo { get; set; }

        [Required(ErrorMessage = "El campo periodo es obligatorio")]
        [Display(Name = "Periodo")]
        public byte PeriodoId { get; set; }

        public Periodo Periodo { get; set; }


        [Required(ErrorMessage = "El campo termina en es obligatorio")]
        [Display(Name = "Termina en")]
        public DateTime FechaFin { get; set; }


    }
}