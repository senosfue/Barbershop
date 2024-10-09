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
        public Task<Response<Haircut>> DeleteAsyn(int id);//
        public Task<Response<Haircut>> EditAsyn(Haircut haircut);//
        public Task<Response<List<Haircut>>> GetListAsync();
        public Task<Response<Haircut>> GetOneAsync(int id);//
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

        public async Task<Response<Haircut>> EditAsyn(Haircut model)
        {
            try
            {
                _context.Haircuts.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Haircut>.MakeResponseSuccess(model, "Haircut actualizado con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Haircut>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Haircut>> DeleteAsyn(int id)
        {
            try
            {
                Response<Haircut> response = await GetOneAsync(id);
                if (!response.IsSuccess)
                {
                    return response;
                }
                _context.Haircuts.Remove(response.Result);
                await _context.SaveChangesAsync();
                return ResponseHelper<Haircut>.MakeResponseSuccess(null, "seccion eliminada con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Haircut>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Haircut>> GetOneAsync(int id)
        {
            try
            {
                Haircut? haircut = await _context.Haircuts.FirstOrDefaultAsync(s => s.Id == id);

                if (haircut is null)
                {
                    return ResponseHelper<Haircut>.MakeResponseFail(null, "la seccion con el id indicado no existe");
                }

                return ResponseHelper<Haircut>.MakeResponseSuccess(haircut);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Haircut>.MakeResponseFail(ex);
            }
        }
    }
}
