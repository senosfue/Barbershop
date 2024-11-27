using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.DTOs;
using BarberShop.Web.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Web.Services
{
    public interface IAppointmentServices
    {
        public Task<Response<Appointment>> CreateAsyn(AppointmentDTO dto);
        public Task<Response<Appointment>> DeleteAsyn(int id);//
        public Task<Response<Appointment>> EditAsyn(AppointmentDTO dto);//
        public Task<Response<PaginationResponse<Appointment>>> GetListAsync(PaginationRequest request);
        public Task<Response<Appointment>> GetOneAsync(int id);//

    }

    public class AppointmentServices : IAppointmentServices
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        public AppointmentServices(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Appointment>> CreateAsyn(AppointmentDTO dto)
        {
            try
            {
                Appointment Appointment = _converterHelper.ToAppointment(dto);
                Appointment.Haircut = await _context.Haircuts.FirstOrDefaultAsync(c => c.Id == dto.IdHaircut);
                Console.WriteLine($"Guardando corte: {Appointment.Id}");
                await _context.Appointments.AddAsync(Appointment);
                await _context.SaveChangesAsync();

                return ResponseHelper<Appointment>.MakeResponseSuccess(Appointment, "corte ingresado con exito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                Console.WriteLine($"Detalles de la excepción interna: {ex.InnerException?.Message}");
                return ResponseHelper<Appointment>.MakeResponseFail(ex);
            }
        }



        public async Task<Response<PaginationResponse<Appointment>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Appointment> query = _context.Appointments.AsQueryable();

                

                PagedList<Appointment> list = await PagedList<Appointment>.ToPagedListAsync(query, request);

                PaginationResponse<Appointment> result = new PaginationResponse<Appointment>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Appointment>>.MakeResponseSuccess(result, "Appointments obtenidos con exito.");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Appointment>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Appointment>> EditAsyn(AppointmentDTO dto)
        {
            try
            {
                Appointment Appointment = await _context.Appointments.FirstOrDefaultAsync(b => b.Id == dto.Id);



                //blog = _converterHelper.ToBlog(dto);

                Appointment.Time = dto.Time;
                
                Appointment.IdHaircut = dto.IdHaircut;


                _context.Appointments.Update(Appointment);
                await _context.SaveChangesAsync();

                return ResponseHelper<Appointment>.MakeResponseSuccess(Appointment, "Blog actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Appointment>.MakeResponseFail(ex);
            }
        }


        public async Task<Response<Appointment>> DeleteAsyn(int id)
        {
            try
            {
                Response<Appointment> response = await GetOneAsync(id);
                if (!response.IsSuccess)
                {
                    return response;
                }
                _context.Appointments.Remove(response.Result);
                await _context.SaveChangesAsync();
                return ResponseHelper<Appointment>.MakeResponseSuccess(null, "seccion eliminada con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Appointment>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Appointment>> GetOneAsync(int id)
        {
            try
            {
                Appointment? Appointment = await _context.Appointments.FirstOrDefaultAsync(s => s.Id == id);

                if (Appointment is null)
                {
                    return ResponseHelper<Appointment>.MakeResponseFail(null, "la seccion con el id indicado no existe");
                }

                return ResponseHelper<Appointment>.MakeResponseSuccess(Appointment);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Appointment>.MakeResponseFail(ex);
            }
        }
    }
}
