using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa un nombre para la categoria")]
        [Display(Name = "Nombre categoria")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Orden de visualizacion")]
        public int Orden { get; set; }

    }
}
