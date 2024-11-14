using ElasticSearch_API.Models;

namespace ElasticSearch_API.DTOs
{
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
        public Product CreateProduct()
        {
            var product = new Product() 
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = (EColor) int.Parse(Feature.Color),
                }
            };
            return product;
        }
    }
}
