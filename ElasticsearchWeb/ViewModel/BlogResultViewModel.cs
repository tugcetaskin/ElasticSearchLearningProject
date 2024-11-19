using System.ComponentModel.DataAnnotations;

namespace ElasticsearchWeb.ViewModel
{
    public class BlogResultViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Blog Title")]
        public string Title { get; set; } = null!;
        [Display(Name = "Blog Content")]
        public string Content { get; set; } = null!;
        [Display(Name = "Blog Tags")]
        public string Tags { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
