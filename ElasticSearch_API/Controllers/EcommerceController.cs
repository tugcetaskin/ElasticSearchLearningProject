using ElasticSearch_API.Models.ECommerceModel;
using ElasticSearch_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace ElasticSearch_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EcommerceController : ControllerBase
    {
        private readonly EcommerceRepository _repository;

        public EcommerceController(EcommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            var result = await _repository.TermQuery(customerFirstName);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstName)
        {
            var result = await _repository.TermsQuery(customerFirstName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            var result = await _repository.PrefixQuery(customerFullName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _repository.RangeQuery(fromPrice, toPrice);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            var result = await _repository.MatchAllQuery();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page=1, int pageSize=10)
        {
            var result = await _repository.PaginationQuery(page, pageSize);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> WildcardQuery(string customerFullName)
        {
            var result = await _repository.WildcardQuery(customerFullName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerFirstName, int fuzzy)
        {
            var result = await _repository.FuzzyQuery(customerFirstName, fuzzy);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string categoryName)
        {
            var result = await _repository.MatchQueryFullText(categoryName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixFullText(string customerFullName)
        {
            var result = await _repository.MatchBoolPrefixFullText(customerFullName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MatchPharaseFullText(string customerFullName)
        {
            var result = await _repository.MatchPharaseFullText(customerFullName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExpOne(string cityName, double taxfulPrice, string category, string manufacturar)
        {
            var result = await _repository.CompoundQueryExpOne(cityName, taxfulPrice, category, manufacturar);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExpOTwo(string customerFullName)
        {
            var result = await _repository.CompoundQueryExpOTwo(customerFullName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MultiMatchFullText(string data)
        {
            var result = await _repository.CompoundQueryExpOTwo(data);
            return Ok(result);
        }
    }
}
