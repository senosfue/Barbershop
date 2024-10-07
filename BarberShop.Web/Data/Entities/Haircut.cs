using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.Data.Entities
{
    public class Haircut
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Corte")]
        [MaxLength(32,ErrorMessage = "Este campo '{0}' es obligatorio")]
        public string Name { get; set; }
        
        [Display(Name = "Calificacion")]
        [Required(ErrorMessage = "Este campo '{0}' es obligatorio")]
        public int Rating {  get; set; }
        public Category Category { get; set; }
        
        public int IdCategory { get; set; }
    }
}
