using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Display (Name = "Categoria")]
        [MaxLength(32,ErrorMessage = "Este campo '{0}' es obligatorio")]
        public string CategoryName { get; set; } //Nombre de la categoria


        [Display (Name = "Descripcion")]
        [MaxLength(32)]
        public string? Description { get; set; } //una breve descripcion


        [Display(Name = "En Tendencia")]
        [Required(ErrorMessage = "Este campo '{0}' es obligatorio")]
        public bool Trending { get; set; } // Si el corte es tendencia 


        [Display(Name = "Edad Del Estilo")]
        [MaxLength(32)]
        public string AgeGroup { get; set; } //una breve descripcion



    }
}
