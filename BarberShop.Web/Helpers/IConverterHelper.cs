using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using System.Reflection.Metadata;

namespace BarberShop.Web.Helpers
{
    public interface IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto);
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
    }
    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper? _combosHelper;
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

        
       public async Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true)
       {
                return new UserDTO
                {
                    id = isNew ? Guid.NewGuid() : Guid.Parse(user.Id),
                    Document = user.Document,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync(),
                    BarberShopRoleId = user.BarberShopRoleId,
                    PhoneNumber = user.PhoneNumber,
                };
       }
        
    }
}
