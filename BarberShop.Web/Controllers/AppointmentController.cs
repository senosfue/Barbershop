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
    public class AppointmentController : Controller
    {
        private readonly IAppointmentServices _AppointmentServices;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        public AppointmentController(IAppointmentServices AppointmentServices, INotyfService notifyService, ICombosHelper combosHelper)
        {
            _AppointmentServices = AppointmentServices;
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
            Response<PaginationResponse<Appointment>> response = await _AppointmentServices.GetListAsync(request);
            return View(response.Result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            AppointmentDTO dto = new AppointmentDTO
            {
                Haircuts = await _combosHelper.GetCombosHaircut(),

            };
            return View(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AppointmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("debe de ajustar los errore de validadcion");
                dto.Haircuts = await _combosHelper.GetCombosHaircut();
                return View(dto);
            }
            Response<Appointment> response = await _AppointmentServices.CreateAsyn(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.Haircuts = await _combosHelper.GetCombosHaircut();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Appointment> response = await _AppointmentServices.GetOneAsync(id);

            if (response.IsSuccess)
            {
                AppointmentDTO dto = new AppointmentDTO
                {
                    Id = response.Result.Id,
                    Time = response.Result.Time,
                    
                    // Mapear otros atributos de Appointment a AppointmentDTO según corresponda
                    Haircuts = await _combosHelper.GetCombosHaircut() // Si es necesario
                };
                return View(dto);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(AppointmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validacion");
                    dto.Haircuts = await _combosHelper.GetCombosHaircut(); // Cargar categorías si es necesario
                    return View(dto);
                }

                Appointment Appointment = new Appointment
                {
                    Id = dto.Id,
                    Time = dto.Time,
                    
                    IdHaircut = dto.IdHaircut,

                };

                Response<Appointment> response = await _AppointmentServices.EditAsyn(dto);

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
            Response<Appointment> response = await _AppointmentServices.DeleteAsyn(id);

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
