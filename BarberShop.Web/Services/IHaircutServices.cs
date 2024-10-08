using BarberShop.Web.Core;
using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Services
{
    public interface IHaircutServices
    {
        public Task<Response<Haircut>> CreateAsyn(HaircutDTO dto );
        public Task<Response<List<Haircut>>> GetListAsync();
    }
    public class HaircutServices : IHaircutServices
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        public HaircutServices(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Haircut>> CreateAsyn(HaircutDTO dto)
        {
            try
            {
                Haircut haircut = _converterHelper.ToHaircut(dto);
                haircut.Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.IdCategory);
                Console.WriteLine($"Guardando corte: {haircut.Name}");
                await _context.Haircuts.AddAsync(haircut);
                await _context.SaveChangesAsync();

                return ResponseHelper<Haircut>.MakeResponseSuccess(haircut, "corte ingresado con exito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                Console.WriteLine($"Detalles de la excepción interna: {ex.InnerException?.Message}");
                return ResponseHelper<Haircut>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<Haircut>>> GetListAsync()
        {
            try
            {
                List<Haircut> haircuts = await _context.Haircuts.Include(b=>b.Category).ToListAsync();

                return ResponseHelper<List<Haircut>>.MakeResponseSuccess(haircuts);
            }
            catch (Exception ex)
            {
                return ResponseHelper < List <Haircut>>.MakeResponseFail(ex);
            }
        }
    }
}
