using BarberShop.Web.Core;
using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Services
{
    public interface IHaircutServices
    {
        public Task<Response<Haircut>> CreateAsyn(Haircut model);
        public Task<Response<List<Haircut>>> GetListAsync();
    }
    public class HaircutServices : IHaircutServices
    {
        private readonly DataContext _context;

        public HaircutServices(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Haircut>> CreateAsyn(Haircut model)
        {
            try
            {
                Haircut haircut = new Haircut 
                { 
                    Name = model.Name,
                    Id = model.Id,
                    Rating = model.Rating,
                    IdCategory = model.IdCategory,
                };
                await _context.Haircuts.AddAsync(haircut);
                await _context.SaveChangesAsync();
                return ResponseHelper<Haircut>.MakeResponseSuccess(haircut, "corte ingresado con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Haircut>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<Haircut>>> GetListAsync()
        {
            try
            {
                List<Haircut> haircuts = await _context.Haircuts.ToListAsync();

                return ResponseHelper<List<Haircut>>.MakeResponseSuccess(haircuts);
            }
            catch (Exception ex)
            {
                return ResponseHelper < List <Haircut>>.MakeResponseFail(ex);
            }
        }
    }
}
