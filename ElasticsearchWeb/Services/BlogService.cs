using ElasticsearchWeb.Models;
using ElasticsearchWeb.Repository;
using ElasticsearchWeb.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Immutable;

namespace ElasticsearchWeb.Services
{
    public class BlogService
    {
        //Dependency inversion | Inversion of Controlf => Dependency Injection
        //Bu 2 prensibi pratikte uygularken Dependency Injection Design Pattern dan yararlanıyoruz.
        private readonly BlogRepository _repository;

        public BlogService(BlogRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel blog)
        {
            Blog saveBlog = new Blog()
            {
                UserId = Guid.NewGuid(),
                Title = blog.Title,
                Tags = blog.Tags.Split(","),
                Content = blog.Content
            };
            var result = await _repository.SaveAsync(saveBlog);
            return result != null;
        }

        public async Task<List<BlogResultViewModel>> SearchAsync(string data)
        {
            var result = await _repository.SearchAsync(data);
            return result.Select(blog => new BlogResultViewModel()
            {
				Id = blog.Id,
				Title = blog.Title,
				Content = blog.Content,
				Tags = string.Join(",", blog.Tags),
				Created = blog.Created
			}).ToList();
			//var list = new List<BlogResultViewModel>();
			//foreach (var item in result)
			//{
			//    list.Add(new BlogResultViewModel()
			//    {
			//        Id = item.Id,
			//        Title = item.Title,
			//        Content = item.Content,
			//        Tags = string.Join(",", item.Tags),
			//        Created = item.Created
			//    });
			//}

			//return list;
		}
    }
}
