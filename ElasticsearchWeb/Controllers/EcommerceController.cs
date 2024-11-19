using ElasticsearchWeb.Services;
using ElasticsearchWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchWeb.Controllers
{
	public class EcommerceController : Controller
	{
		private readonly EcommerceService _service;

		public EcommerceController(EcommerceService service)
		{
			_service = service;
		}

		public async Task<IActionResult> Search(SearchPageViewModel searchPage)
		{
			var (list, totalcount, pageLinkCount) = await _service.SearchAsync(searchPage.EcommerceSearch, searchPage.Page, searchPage.PageSize);

			searchPage.DataList = list;
			searchPage.TotalCount = totalcount;
			searchPage.PageLinkCount = pageLinkCount;
			return View(searchPage);
		}
	}
}
