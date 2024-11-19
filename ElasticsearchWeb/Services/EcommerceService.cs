using ElasticsearchWeb.Repositories;
using ElasticsearchWeb.ViewModel;

namespace ElasticsearchWeb.Services
{
	public class EcommerceService
	{
		private readonly EcommerceRepository _repository;

		public EcommerceService(EcommerceRepository repository)
		{
			_repository = repository;
		}
		public async Task<(List<ECommerceResultViewModel> list, long totalCount, long pageLinkCount)> SearchAsync(EcommerceSearchViewModel search, int page, int pagesize)
		{
			//Arama sonucu => list
			//Arama sonuç sayısı => totalCount
			//Kaç sayfa var => 1 2 3 4 5

			var (list, totalCount) = await _repository.SearchAsync(search, page, pagesize);
			var pageLinkCountCal = totalCount % pagesize;
			long pageLinkCount = 0;
			if (pageLinkCountCal == 0)
			{
				pageLinkCount = totalCount / pagesize;
			}
			else
			{
				pageLinkCount = (totalCount / pagesize) + 1;
			}

			var ecommerceListViewModel = list.Select(s => new ECommerceResultViewModel()
			{
				Category = String.Join(",", s.Category),
				CustomerFullName = s.CustomerFullName,
				CustomerFirstName = s.CustomerFirstName,
				CustomerLastName = s.CustomerLastName,
				OrderDate = s.OrderDate,
				Gender = s.Gender,
				Id = s.Id,
				OrderId = s.OrderId,
				TaxfulTotalPrice = s.TaxfulTotalPrice,
			}).ToList();

			return (list: ecommerceListViewModel, totalCount, pageLinkCount);
		}
	}
}
