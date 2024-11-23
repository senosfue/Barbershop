
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.Data;

namespace BarberShop.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<BarberShopRole>> CreateAsync(BarberShopRoleDTO dto);
        public Task<Response<BarberShopRole>> EditAsync(BarberShopRoleDTO dto);
        public Task<Response<PaginationResponse<BarberShopRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<BarberShopRoleDTO>> GetOneAsync(int id);
        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
    }
    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<BarberShopRole>> CreateAsync(BarberShopRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try 
                {
                    //creacion del rol
                    BarberShopRole role =  _converterHelper.ToRole(dto);
                    await _context.BarberShopRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    //insercion de permisos 
                    int roleId = role.Id;
                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);

                    }

                    foreach (int permissionId in permissionIds )
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };
                        
                        await _context.RolePermissions.AddAsync(rolePermission);
                    }
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ResponseHelper<BarberShopRole>.MakeResponseSuccess(role, "Rol creado con exito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<BarberShopRole>.MakeResponseFail(ex); 
                }
            }
        }

        public async Task<Response<BarberShopRole>> EditAsync(BarberShopRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<BarberShopRole>.MakeResponseFail($"El role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado.");
                }

                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de permisos antiguos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                // Actualización de Rol
                BarberShopRole model = _converterHelper.ToRole(dto);
                _context.BarberShopRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<BarberShopRole>.MakeResponseSuccess(model, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<BarberShopRole>.MakeResponseFail(ex);
            }
        }

        public async  Task<Response<PaginationResponse<BarberShopRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {

                IQueryable<BarberShopRole> query = _context.BarberShopRoles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<BarberShopRole> list = await PagedList<BarberShopRole>.ToPagedListAsync(query, request);

                PaginationResponse<BarberShopRole> result = new PaginationResponse<BarberShopRole>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<BarberShopRole>>.MakeResponseSuccess(result, "Roles obtenidos con exito.");

            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<BarberShopRole>>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<BarberShopRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                BarberShopRole? role = await _context.BarberShopRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<BarberShopRoleDTO>.MakeResponseFail(null, "la seccion con el id indicado no existe");
                }

                return ResponseHelper<BarberShopRoleDTO>.MakeResponseSuccess(await _converterHelper.TORoleDTOAsync(role));
            }
            catch (Exception ex)
            {
                return ResponseHelper<BarberShopRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try 
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<BarberShopRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
