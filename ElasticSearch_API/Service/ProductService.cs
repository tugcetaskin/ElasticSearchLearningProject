using ElasticSearch_API.DTOs;
using ElasticSearch_API.Models;
using ElasticSearch_API.Repository;
using Nest;
using System.Collections.Immutable;
using System.Net;

namespace ElasticSearch_API.Service
{
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _repository.SaveAsync(request.CreateProduct());
            
            if(response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> {"Kayıt esnasında bir hata meydana geldi."}, HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(response.CreateProductDto(), HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            var productListDto = new List<ProductDto>();

            foreach (var x in products)
            {
                if(x.Feature == null)
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
                else
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature.Color.ToString())));

            }
            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var response = await _repository.GetByIdAsync(id);
            if (response == null)
                ResponseDto<ProductDto>.Fail("Aranılan veri bulunamadı.", HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Success(response.CreateProductDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto update)
        {
            var response = await _repository.UpdateAsync(update);
            if (response == false)
                return ResponseDto<bool>.Fail("Veri Güncellenemedi", HttpStatusCode.InternalServerError);
            return ResponseDto<bool>.Success(response, HttpStatusCode.NoContent);
        }
        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var response = await _repository.DeleteAsync(id);

            if(!response.IsValid && response.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail("Veri Bulunamadı!", HttpStatusCode.NotFound);

            }
            if(!response.IsValid)
            {
                _logger.LogError(response.OriginalException, response.ServerError.Error.ToString());
                return ResponseDto<bool>.Fail("Veri Silme işlemi sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);

            }
            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
