using Ecommerce.SharedLibrary.Common;
using Ecommerce.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new Response { Success = true, Message = "Product added successfully" };
            }catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                return new Response { Success = false, Message = "Error occurred while adding new product" };

            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var exist = await GetByIdAsync(entity.Id);
                if (exist is null)
                {
                    return new Response(false, $"{entity.Name} does not exist");
                }
                _context.Products.Remove(exist);
                await _context.SaveChangesAsync();
                return new Response { Success = true, Message = $"{entity.Name} is deleted successfully" };
            }
            catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                return new Response { Success = false, Message = $"Error occurred while removing {entity.Name}" };

            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var product = await _context.Products.AsNoTracking().ToListAsync();
                // using ternary operator
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                throw new Exception("Error occurred while retrieving products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _context.Products.Where(predicate).FirstOrDefaultAsync();
                // using ternary operator
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                throw new Exception("Error occurred while retrieving product");
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                // using ternary operator
                return product is not null? product : null;
            }
            catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                //return new Response { Success = false, Message = "Error occur retrieving the product" };
                throw new Exception("Error occurred while retrieving the product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await GetByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, $"{entity.Name} does not exist");
                }
                _context.Entry(product).State = EntityState.Detached;
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return new Response { Success = true, Message = $"{entity.Name} is updated successfully" };
            }
            catch (Exception ex)
            {
                // log the original exception
                LogException.LogExceptions(ex);
                // display error mesage to the client
                return new Response { Success = false, Message = $"Error occurred while updating {entity.Name}" };

            }
        }
    }
}
