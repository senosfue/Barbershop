using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;


namespace BarberShop.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersServices _usersService;

        public UsersController(ICombosHelper combosHelper, INotyfService notifyService, IUsersServices usersService)
        {
            _combosHelper = combosHelper;
            _notifyService = notifyService;
            _usersService = usersService;
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
    }
}
