using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using System.Reflection.Metadata;

namespace BarberShop.Web.Helpers
{
    public interface IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto);
    }
    public class ConverterHelper : IConverterHelper
    {
        public Haircut ToHaircut(HaircutDTO dto)
        {
            return new Haircut
            {
                Name= dto.Name,
                Id = dto.Id,
                Rating = dto.Rating,
                IdCategory = dto.IdCategory,
            };
        }
    }
}
