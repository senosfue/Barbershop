using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using System.Reflection.Metadata;

namespace BarberShop.Web.Helpers
{
    public interface IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto);
        public User ToUser(UserDTO dto);
    }
    public class ConverterHelper : IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto)
        {
            return new Haircut
            {
                Name= dto.Name,
                Id = dto.Id,
                Rating = dto.Rating,
                IdCategory = dto.IdCategory,
            };
        }

        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.id.ToString(),
                Document = dto.Document,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                BarberShopRoleId = dto.BarberShopRoleId,
                PhoneNumber = dto.PhoneNumber,
            };
        }
    }
}
