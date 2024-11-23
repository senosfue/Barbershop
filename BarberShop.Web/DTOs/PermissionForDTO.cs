using BarberShop.Web.Data.Entities;

namespace BarberShop.Web.DTOs
{
    public class PermissionForDTO : Permission
    {
        public bool Selected { get; set; }
    }
}