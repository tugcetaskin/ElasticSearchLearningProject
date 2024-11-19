namespace ElasticsearchWeb.ViewModel
{
	public class SearchPageViewModel
	{
        public long TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
		public long PageLinkCount { get; set; }
		public List<ECommerceResultViewModel> DataList { get; set; }
		public EcommerceSearchViewModel EcommerceSearch { get; set; }
		public int StartPage()
		{
			return Page - 6 <= 0 ? 1 : Page - 6;
		}
        public long EndPage()
        {
            return Page + 6 >= PageLinkCount ? PageLinkCount : Page + 6;
        }
        public string CreatePageUrl(HttpRequest request, long page, int pageSize)
		{
			//https://localhost/search?sfcd
			var currentUrl= new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsoluteUri;
			if (currentUrl.Contains("page", StringComparison.OrdinalIgnoreCase))
			{
				currentUrl = currentUrl.Replace($"Page={Page}", $"Page={page}", StringComparison.OrdinalIgnoreCase);
				currentUrl = currentUrl.Replace($"PageSize={Page}", $"Page={pageSize}", StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				currentUrl = $"{currentUrl}?Page={page}";
				currentUrl = $"{currentUrl}&PageSize={pageSize}";
			}
			return currentUrl;
		}
	}
}
