using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using BarberShop.Web.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
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
            Response<PaginationResponse<Haircut>> response = await _haircutServices.GetListAsync(request);
            return View(response.Result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            HaircutDTO dto = new HaircutDTO
            {
                Categorys = await _combosHelper.GetCombosCategory(),
            
            };
            return View(dto);
        }

        [HttpPost]
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute]int id)
        {
            Response<Haircut> response = await _haircutServices.GetOneAsync(id);

            if (response.IsSuccess)
            {
                HaircutDTO dto = new HaircutDTO
                {
                    Id = response.Result.Id,
                    Name = response.Result.Name,
                    Rating = response.Result.Rating,
                    // Mapear otros atributos de Haircut a HaircutDTO según corresponda
                    Categorys = await _combosHelper.GetCombosCategory() // Si es necesario
                };
                return View(dto);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(HaircutDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    dto.Categorys = await _combosHelper.GetCombosCategory(); // Cargar categorías si es necesario
                    return View(dto);
                }

                Haircut haircut = new Haircut
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Rating = dto.Rating,
                    IdCategory = dto.IdCategory,
                    
                };

                Response<Haircut> response = await _haircutServices.EditAsyn(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(dto);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Haircut> response = await _haircutServices.DeleteAsyn(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }
            else
            {
                _notifyService.Error(response.Message);
            }


            return RedirectToAction(nameof(Index));

        }

       

    }
}
    

