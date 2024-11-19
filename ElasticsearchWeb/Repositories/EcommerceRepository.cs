using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticsearchWeb.Models;
using ElasticsearchWeb.ViewModel;
using System.Drawing.Printing;

namespace ElasticsearchWeb.Repositories
{
	public class EcommerceRepository
	{
		private readonly ElasticsearchClient _client;
		private const string indexName = "kibana_sample_data_ecommerce";

		public EcommerceRepository(ElasticsearchClient client)
		{
			_client = client;
		}

		public async Task<(List<ECommerce> list, long count)> SearchAsync(EcommerceSearchViewModel search, int page, int pagesize)
		{
			List<Action<QueryDescriptor<ECommerce>>> list = new();

			if(search is null)
			{
				list.Add(q => q.MatchAll(m => { }));
				return await CalculateResultSet(page, pagesize, list);
            }
			if(!string.IsNullOrEmpty(search.Category))
			{
				list.Add(Query => Query.Match(m => m.
						Field(f => f.Category).
							Query(search.Category)));
			}
			if (!string.IsNullOrEmpty(search.CustomerFullName))
			{
				list.Add(Query => Query.Match(m => m.
						Field(f => f.CustomerFullName).
							Query(search.CustomerFullName)));
			}
			if (search.OrderDateStart.HasValue)
			{
				list.Add(Query => Query.Range(r => r.
					DateRange(d => d.
						Field(f => f.OrderDate).
							Gte(search.OrderDateStart.Value))));
			}
			if (search.OrderDateEnd.HasValue)
			{
				list.Add(Query => Query.Range(r => r.
					DateRange(d => d.
						Field(f => f.OrderDate).
							Lte(search.OrderDateEnd.Value))));
			}
			if (!string.IsNullOrEmpty(search.Gender))
			{
				list.Add(Query => Query.Term(t => t.
					Field(f => f.Gender).
						Value(search.Gender)));
			}

			if(list.Any())
			{
                list.Add(q => q.MatchAll(m => { }));
            }

            return await CalculateResultSet(page, pagesize, list);
		}

		public async Task<(List<ECommerce> list, long count)> CalculateResultSet(int page, int pageSize, List<Action<QueryDescriptor<ECommerce>>> list)
		{
            var pageFrom = (page - 1) * pageSize;
            var result = await _client.SearchAsync<ECommerce>(search => search.Index(indexName)
            .Size(pageSize).From(pageFrom)
            .Query(q => q.Bool(b => b
            .Must(list.ToArray()))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return (list: result.Documents.ToList(), result.Total);
        }
	}
}
