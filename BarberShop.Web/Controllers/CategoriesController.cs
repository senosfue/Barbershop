using AspNetCoreHero.ToastNotification.Abstractions;
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.Helpers;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Controllers
{
    //[Authorize]
    //[AllowAnonymous] para excluir uno 
    public class CategoriesController : Controller
    {

        private readonly ICategoriesService _categoriesService;
        private readonly INotyfService _notifyService;

        public CategoriesController(ICategoriesService categoriesService, INotyfService notifyService)
        {
            _categoriesService = categoriesService;
            _notifyService = notifyService;
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
            Response<PaginationResponse<Category>> response = await _categoriesService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Category category)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return View(category);


                }

                Response<Category> response = await _categoriesService.CreateAsync(category);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }


                // TODO: MOSTRAR MENSAJE DE ERROR 

                return View(response);


            }
            catch (Exception ex)
            {
                return View(category);
            }


        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Category> response = await _categoriesService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de valiadacion");
                    return View(category);
                }

                Response<Category> response = await _categoriesService.EditAsyn(category);

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
                _notifyService.Error(ex.Message);
                return View(category);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Category> response = await _categoriesService.DeleteAsyn(id);

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