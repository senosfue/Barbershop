using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.Data.Entities
{
    public class BarberShopRole
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } 
    }
}