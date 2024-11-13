using Azure.Core;
using BarberShop.Web.Core;
using BarberShop.Web.Core.Pagination;
using BarberShop.Web.Data;
using BarberShop.Web.Data.Entities;
using BarberShop.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace BarberShop.Web.Services
{
    public interface ICategoriesService
    {

        public Task<Response<Category>> CreateAsync(Category model);
        public Task<Response<Category>> DeleteAsyn(int id);
        public Task<Response<Category>> EditAsyn(Category model);
        public Task<Response<PaginationResponse<Category>>> GetListAsync(PaginationRequest request);
        public Task<Response<Category>> GetOneAsync(int id);


    }


    public class CategoriesService : ICategoriesService
    {

        private readonly DataContext _context;

        public CategoriesService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Category>> CreateAsync(Category model)
        {
            try
            {
                Category category = new Category
                {

                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    Trending = model.Trending,
                    AgeGroup = model.AgeGroup

                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return ResponseHelper<Category>.MakeResponseSuccess(category, "! Categoria creada con exito ¡");

            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<Category>> DeleteAsyn(int id)
        {
            try
            {
                Response<Category> response = await GetOneAsync(id);
                if (!response.IsSuccess)
                {
                    return response;
                }
                _context.Categories.Remove(response.Result);
                await _context.SaveChangesAsync();
                return ResponseHelper<Category>.MakeResponseSuccess(null, "seccion eliminada con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Category>> EditAsyn(Category model)
        {
            try
            {
                _context.Categories.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Category>.MakeResponseSuccess(model, "Categoria actualizada con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Category>>> GetListAsync(PaginationRequest request)
        {

            try
            {

                IQueryable<Category> query = _context.Categories.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.CategoryName.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Category> list = await PagedList<Category>.ToPagedListAsync(query, request);

                PaginationResponse<Category> result = new PaginationResponse<Category>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Category>>.MakeResponseSuccess(result, "Categorias obtenidos con exito.");

            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Category>>.MakeResponseFail(ex);

            }
        }


        public async Task<Response<Category>> GetOneAsync(int id)
        {
            try
            {
                Category? category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

                if (category is null)
                {
                    return ResponseHelper<Category>.MakeResponseFail(null,"la seccion con el id indicado no existe");
                }

                return ResponseHelper<Category>.MakeResponseSuccess(category);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Category>.MakeResponseFail(ex);
            }
        }

    }
}