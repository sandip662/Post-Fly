﻿using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Name can only contain letters, numbers, and underscores")]
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
