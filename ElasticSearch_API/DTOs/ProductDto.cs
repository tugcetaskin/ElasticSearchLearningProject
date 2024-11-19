using ElasticSearch_API.Models;

namespace ElasticSearch_API.DTOs
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {
    }
}
