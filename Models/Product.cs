using System.ComponentModel.DataAnnotations;

namespace DotNet_MinimalAPI_Example.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}
    }
}
