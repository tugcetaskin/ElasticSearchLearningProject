using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch_API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearch_API.Repository
{
    public class EcommerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";

        public EcommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<ECommerce>> TermQuery(string customerFisrtName)
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName)
                .Query(
                    q => q.Term(
                        t => t.Field("customer_first_name.keyword").Value(customerFisrtName))));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermQuerySecond(string customerFisrtName)
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName)
                .Query(
                    q => q.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFisrtName)));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermQueryThird(string customerFisrtName)
        {
            var termQuery = new TermQuery("customer_first_name.keyword")
            {
                Value = customerFisrtName,
                CaseInsensitive = true
            };

            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName)
                    .Query(termQuery));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFisrtNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFisrtNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            var termsQuery = new TermsQuery()
            {
                Field = "customer_first_name.keyword",
                Terms = new TermsQueryField(terms.AsReadOnly())
            };

            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).Query(termsQuery));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQuerySecond(List<string> customerFisrtNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFisrtNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).Size(100).
                Query(q => q.
                Terms(t => t.
                Field(f => f.CustomerFirstName.Suffix("keyword")).
                Terms(new TermsQueryField(terms.AsReadOnly())))));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQuery(string word)
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).
                Query(q => q.
                Prefix(p => p.
                Field(f => f.CustomerFullName.Suffix("keyword")).
                Value(word))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).
                Query(q => q.
                Range(r => r.
                NumberRange(nr => nr.
                Field(f => f.TaxfulTotalPrice).
                Gte(fromPrice).Lte(toPrice)))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).Size(100).
                Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PaginationQuery(int page, int pageSize)
        {
            //page=1 | pageSize=10 => 1 - 10
            //page=2 | pageSize=10 => 11 - 20
            var pageFrom = (page - 1) * pageSize;

            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName).Size(pageSize).From(pageFrom).
                Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> WildcardQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
            Wildcard(w => w.
            Field(f => f.CustomerFullName.Suffix("keyword")).
            Wildcard(customerFullName))));

            foreach(var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQuery(string customerFirstName, int fuzzyNum)
        {
            Fuzziness fuzzy = new Fuzziness(fuzzyNum);
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
            Fuzzy(u => u.
            Field(f => f.CustomerFirstName.Suffix("keyword")).
            Value(customerFirstName).Fuzziness(fuzzy))).
            Sort(sort => sort.
            Field(x => x.TaxfulTotalPrice,
                new FieldSort() { Order = SortOrder.Desc})));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchQueryFullText(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
            Match(m => m.
            Field(f => f.Category).
            Query(categoryName))));

            foreach(var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextWithAnd(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
            Match(m => m.
            Field(f => f.Category).
            Query(categoryName).
            Operator(Operator.And))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.MatchBoolPrefix(m => m.
            Field(f => f.CustomerFullName).Query(customerFullName))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchPharaseFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.MatchPhrase(m => m.
            Field(f => f.CustomerFullName).Query(customerFullName))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MultiMatchFullText(string data)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10)
            .Query(q => q.
                MultiMatch(m => m.
                    Fields(new Field("customer_first_name").
                        And(new Field("customer_last_name")).
                        And(new Field("customer_full_name"))).
                    Query(data))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> CompoundQueryExpOne(string cityName, double taxfulPrice, string category, string manufacturar)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
            Bool(b => b.
                Must(m => m.
                    Term(t => t.
                    Field("geoip.city_name").Value(cityName))).
                MustNot(mn => mn.
                    Range(r => r.NumberRange(nr => nr.
                    Field(fi => fi.TaxfulTotalPrice).Lte(taxfulPrice)))).
                Should(s => s.
                    Term(t => t.
                    Field(f => f.Category.Suffix("keyword")).Value(category))).
                Filter(ft => ft.
                    Term(t => t.
                    Field("manufacturer.keyword").Value(manufacturar))))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExpOTwo(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Query(q => q.
                MatchPhrasePrefix(m => m.
                    Field(f => f.CustomerFullName).Query(customerFullName))));
            //var result = await _client.SearchAsync<ECommerce>(s => s.
            //Index(indexName).
            //Query(q => q.
            //Bool(b => b.
            //    Should(m => m.
            //        Match(ma => ma.
            //            Field(f => f.CustomerFullName).
            //                Query(customerFullName)).
            //        Prefix(p => p.
            //            Field(f => f.CustomerFullName.Suffix("keyword")).Value(customerFullName))))));

            foreach (var item in result.Hits) item.Source.Id = item.Id;
            return result.Documents.ToImmutableList();
        }
    }
}
