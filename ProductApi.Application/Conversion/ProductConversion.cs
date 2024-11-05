using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;

namespace ProductApi.Application.Conversion
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDto productDto) => new()
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Category = productDto.Category,
            Description = productDto.Description,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
        };

        public static (ProductDto, IEnumerable<ProductDto>) FromEntity(Product product, IEnumerable<Product> products)
        {
            //return single
            if(product is not null || products is null)
            {
                var singleProduct = new ProductDto
                    (product.Id
                    , product.Name
                    , product.Category
                    , product.Description
                    , (int)product.Price
                    , product.Quantity);
                return (singleProduct, null);
            }

            //return list
            if(products is not null || product is null)
            {
                var prods = products.Select(p =>
                new ProductDto(p.Id, p.Name, p.Category, p.Description, (int)p.Price, p.Quantity)).ToList();

                return (null, prods);
            }
            return (null, null);
        }
    }
}
