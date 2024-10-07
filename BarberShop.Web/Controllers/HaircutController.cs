using BarberShop.Web.Core;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Controllers
{
    public class HaircutController : Controller
    {
        private readonly IHaircutServices _haircutServices;

        public HaircutController(IHaircutServices haircutServices)
        {
            _haircutServices = haircutServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<Haircut>> response = await _haircutServices.GetListAsync();
            return View(response.Result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Haircut haircut)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(haircut);
                }
                Response<Haircut> response = await _haircutServices.CreateAsyn(haircut);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                return View(response);
            }
            catch (Exception ex)
            {
                return View(haircut);
            }
        }
    }
}
