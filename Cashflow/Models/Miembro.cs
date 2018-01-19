using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cashflow.Models
{
    [Table("Miembros")]
    public class Miembro
    {
        [Required(ErrorMessage = "El campo miembro es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo miembro es obligatorio")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}