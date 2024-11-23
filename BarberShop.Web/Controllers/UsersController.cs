using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;


namespace BarberShop.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersServices _usersService;
        private readonly IConverterHelper _converterHelper;
        public UsersController(ICombosHelper combosHelper, INotyfService notifyService, IUsersServices usersService, IConverterHelper converterHelper)
        {
            _combosHelper = combosHelper;
            _notifyService = notifyService;
            _usersService = usersService;
            _converterHelper = converterHelper;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            _notifyService.Success("This is a Success Notification");

            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };
            Response<PaginationResponse<User>> response = await _usersService.GetListAsync(request);
            return View(response.Result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserDTO dto = new UserDTO
            {
               BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync(),
            };

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("This is an Error Notification");
                    dto.BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync();
                    return View(dto);
                }
                Response<User> response = await _usersService.CreateAsyn(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }
                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                dto.BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync();
                return View(dto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _usersService.GetUserAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UserDTO dto = await _converterHelper.ToUserDTOAsync(user, false);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync();
                return View(dto);
            }

            Response<User> response = await _usersService.UpdateUserAsync(dto);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            dto.BarberShopRoles = await _combosHelper.GetCombosBarberShopRolesAsync();
            return View(dto);
        }
    }
}
