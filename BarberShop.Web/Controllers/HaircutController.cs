using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.Core;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Controllers
{
    public class HaircutController : Controller
    {
        private readonly IHaircutServices _haircutServices;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        public HaircutController(IHaircutServices haircutServices, INotyfService notifyService, ICombosHelper combosHelper)
        {
            _haircutServices = haircutServices;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _notifyService.Success("This is a Success Notification");
            Response<List<Haircut>> response = await _haircutServices.GetListAsync();
            return View(response.Result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            HaircutDTO dto = new HaircutDTO
            {
                Categorys = await _combosHelper.GetCombosCategory(),
            
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HaircutDTO dto)
        {   
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("debe de ajustar los errore de validadcion");
                    dto.Categorys = await _combosHelper.GetCombosCategory();
                    return View(dto);
                }
                Response<Haircut> response = await _haircutServices.CreateAsyn(dto);

                if (!response.IsSuccess)
                {
                    _notifyService.Error(response.Message);
                    dto.Categorys = await _combosHelper.GetCombosCategory();
                    return View(dto);
                }

                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));

        }
    }
}
