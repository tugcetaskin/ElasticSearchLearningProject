using ElasticsearchWeb.Services;
using ElasticsearchWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchWeb.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Search()
        {
			var list = await _blogService.SearchAsync(string.Empty);
			return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            ViewBag.SearchText = searchText;
            var list = await _blogService.SearchAsync(searchText);
            return View(list);
        }
        public IActionResult Save()
        {
            return View();
        }
        //async sınıflarda bir hata olduğunda Task hatayı kapsüller.
        //O yüzden async metotlar Task sınıfı ile mutlaka wraplanmalı
        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel blog)
        {
            var isSuccess = await _blogService.SaveAsync(blog);

            if(!isSuccess)
            {
                TempData["result"] = "Kayıt Başarısız!";
                return RedirectToAction(nameof(BlogController.Save));
            }
            TempData["result"] = "Kayıt Başarılı!";
            return RedirectToAction(nameof(BlogController.Save));
        }
    }
}
