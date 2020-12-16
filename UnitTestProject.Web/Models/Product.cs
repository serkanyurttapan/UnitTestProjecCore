using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTestProject.Web.Models
{
    public partial class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Gerekli alan")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Minimum 3, Maximum 30 characters")]

        public string Name { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? Stock { get; set; }

        [Required]
        public string Color { get; set; }
    }
}