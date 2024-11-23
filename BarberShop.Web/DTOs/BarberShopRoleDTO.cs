using BarberShop.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.DTOs
{
    public class BarberShopRoleDTO
    {
        
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public List<PermissionForDTO>? Permissions {    get; set; }
        public string? PermissionIds { get; set; }

    }
}
