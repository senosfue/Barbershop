using System.ComponentModel.DataAnnotations;
using BarberShop.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberShop.Web.DTOs
{
    public class HaircutDTO
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Corte")]
        [MaxLength(32, ErrorMessage = "Este campo '{0}' es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Calificacion")]
        [Required(ErrorMessage = "Este campo '{0}' es obligatorio")]
        public int Rating { get; set; }
        [Display(Name = "Categoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public Category? Category { get; set; }

        public int IdCategory { get; set; }
        public IEnumerable<SelectListItem>? Categorys{ get; set; }
    }
}
