using System.ComponentModel.DataAnnotations;

namespace BarberShop.Web.Data.Entities
{
    public class Appointment
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Hora")]
        public DateTime Time {  get; set; }




        public Haircut Haircut { get; set; }

        [Display(Name = "Corte")]
        public int IdHaircut { get; set; }

    }
}
