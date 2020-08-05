using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEntityFrmawork.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo telefono es obligatorio")]
        [Display(Name = "Telefono")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El campo celular es obligatorio")]
        public int Celular { get; set; }

        [Required(ErrorMessage = "El campo email es obligatorio")]
        public string Email { get; set; }

    }
}
