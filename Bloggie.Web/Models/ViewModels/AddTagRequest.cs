using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [MinLength(2, ErrorMessage = "Display Name must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Display Name cannot exceed 100 characters")]
        public string DisplayName
        {
            get; set;
        }
    }

}
