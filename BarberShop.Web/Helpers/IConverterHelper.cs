using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using Microsoft.EntityFrameworkCore;
namespace BarberShop.Web.Helpers
{
    public interface IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto);
        public Appointment ToAppointment(AppointmentDTO dto);
        public BarberShopRole ToRole(BarberShopRoleDTO dto);
        public Task<BarberShopRoleDTO> TORoleDTOAsync(BarberShopRole role);
        public User ToUser(UserDTO dto);
    }
    public class ConverterHelper : IConverterHelper
    {

        private readonly ICombosHelper? _combosHelper;
        private readonly DataContext _context;


        public ConverterHelper(ICombosHelper? combosHelper, DataContext context)
        {
            _combosHelper = combosHelper;
            _context = context;
        }


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

        public Appointment ToAppointment(AppointmentDTO dto)
        {
            return new Appointment
            {
                Time = dto.Time,
                Id = dto.Id,
                
                IdHaircut = dto.IdHaircut,
            };
        }

        public BarberShopRole ToRole(BarberShopRoleDTO dto)
        {
            return new BarberShopRole
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }

        public async Task<BarberShopRoleDTO> TORoleDTOAsync(BarberShopRole role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id),

            }).ToListAsync();

            return new BarberShopRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
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
            try
            {
                UserDTO dto = new UserDTO
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

                return dto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
       }
        
    }
}
