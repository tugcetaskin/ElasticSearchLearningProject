using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticsearchWeb.Models;
using System.Collections.Immutable;

namespace ElasticsearchWeb.Repository
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blogs";

        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Blog> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;
            var result = await _client.IndexAsync(newBlog, i => i.Index(indexName));

            if (!result.IsValidResponse) return null;

            newBlog.Id = result.Id;
            return newBlog;
        }

        public async Task<ImmutableList<Blog>> SearchAsync(string data)
        {
            List<Action<QueryDescriptor<Blog>>> listQuery = new();

			Action<QueryDescriptor<Blog>> matchAll = q => q.MatchAll(m => { });
			Action<QueryDescriptor<Blog>> matchContent = q => q.Match(m => m.
				Field(f => f.Content).Query(data));
			Action<QueryDescriptor<Blog>> matchTitle = q => q.MatchBoolPrefix(m => m.
				Field(f => f.Title).Query(data));
			Action<QueryDescriptor<Blog>> matchTags = q => q.Term(m => m.
				Field(f => f.Tags).Value(data));

			if (string.IsNullOrEmpty(data)) listQuery.Add(matchAll);
            else
            {
                listQuery.Add(matchContent);
                listQuery.Add(matchTitle);
                listQuery.Add(matchTags);
            }

			var result = await _client.SearchAsync<Blog>(s => s.
            Index(indexName).
            Query(q => q.
                Bool(b => b.
                    Should(listQuery.ToArray()))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
    }
}
