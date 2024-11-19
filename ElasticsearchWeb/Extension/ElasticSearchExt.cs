﻿using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearchWeb.Extension
{
    public static class ElasticSearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration) 
        {
            string userName = configuration.GetSection("Elastic")["Username"]!;
            string password = configuration.GetSection("Elastic")["Password"]!;
            var settings = new ElasticsearchClientSettings
                (new Uri(configuration.GetSection("Elastic")["Url"]!))
                .Authentication(new BasicAuthentication(userName!,password!));

            var client = new ElasticsearchClient(settings);
            //var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //var settings = new ConnectionSettings(pool);
            //var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}
