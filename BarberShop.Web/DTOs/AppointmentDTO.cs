using BarberShop.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.DTOs
{
    public class AppointmentDTO
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Hora")]
        public DateTime Time { get; set; }




        [Display(Name = "Corte")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un corte")]
        public Haircut? Haircut { get; set; }





        [Display(Name = "Corte")]
        public int IdHaircut { get; set; }

        public IEnumerable<SelectListItem>? Haircuts { get; set; }
    }
}
