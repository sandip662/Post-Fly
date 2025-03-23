using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
      [Required(ErrorMessage = "Heading is required.")]
        [MaxLength(200, ErrorMessage = "Heading cannot exceed 200 characters.")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "Page title is required.")]
        [MaxLength(200, ErrorMessage = "Page title cannot exceed 200 characters.")]
        public string PageTitle { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Short description is required.")]
        [MaxLength(500, ErrorMessage = "Short description cannot exceed 500 characters.")]
        public string ShortDescription { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string FeaturedImageUrl { get; set; }

        [Required(ErrorMessage = "URL handle is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "URL handle can only contain letters, numbers, and hyphens.")]
        public string UrlHandle { get; set; }

        [Required(ErrorMessage = "Published date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
        public string Author { get; set; }


        public bool Visible { get; set; }


        // Display tags (No validation needed as it's only for UI display)
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect selected tags
        [Required(ErrorMessage = "At least one tag must be selected.")]
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
