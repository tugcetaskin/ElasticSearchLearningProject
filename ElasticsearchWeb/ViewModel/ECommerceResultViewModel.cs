using System.Text.Json.Serialization;

namespace ElasticsearchWeb.ViewModel
{
	public class ECommerceResultViewModel
	{
		public string Id { get; set; } = null!;
		public string CustomerFirstName { get; set; } = null!;
		public string CustomerLastName { get; set; } = null!;
		public string CustomerFullName { get; set; } = null!;
		public string Gender { get; set; } = null!;
		public double TaxfulTotalPrice { get; set; }
		public string Category { get; set; } = null!;
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
