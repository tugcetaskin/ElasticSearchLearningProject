using Elastic.Clients.Elasticsearch;
using ElasticSearch_API.DTOs;
using ElasticSearch_API.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace ElasticSearch_API.Repository
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "products";

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;
            var response = await _client.IndexAsync(newProduct, x => x.Index("products").Id(Guid.NewGuid().ToString()));

            if (!response.IsSuccess()) return null;

            newProduct.Id = response.Id;
            return newProduct;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>
                (p => p.Index(indexName).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var result = await _client.GetAsync<Product> (id, x=>x.Index(indexName));
            result.Source.Id = result.Id;
            return result.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto update)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(indexName, update.Id, x=> x.Doc(update));
            return response.IsSuccess();
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));
            return response;
        }
    }
}
