using Ecommerce.SharedLibrary.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Conversion;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;

namespace ProuctApi.Representation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _product.GetAllAsync();
            if (!products.Any()) return NotFound("No products in the database");
            // using turple
            var (_, list) = ProductConversion.FromEntity(null, products);
            return list.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("Id")]
        public async Task<ActionResult<ProductDto>> GetProduct(int Id)
        {
            var product = await _product.GetByIdAsync(Id);
            if (product is null) return NotFound("No product in the database");

            // using turple, convert from entity to dto
            var (result, _) = ProductConversion.FromEntity(product, null);
            return result is not null ? Ok(result) : NotFound("No product found");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDto product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var getEntity = ProductConversion.ToEntity(product);
            var response = await _product.CreateAsync(getEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDto product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var getEntity = ProductConversion.ToEntity(product);
            var response = await _product.UpdateAsync(getEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDto product)
        {
            var getEntity = ProductConversion.ToEntity(product);
            var response = await _product.DeleteAsync(getEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }
    }
}
