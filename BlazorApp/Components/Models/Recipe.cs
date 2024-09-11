using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class Recipe
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Instructions { get; set; }

        public int Id { get; set; }

        //public string PhotoUrl { get; set; }
    }

}