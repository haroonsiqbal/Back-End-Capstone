using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireSafe.Models
{
    public class Log
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please choose a category.")]
        [Display(Name = "Product Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public double Price { get; set; }
        
        [Required(ErrorMessage = "Please choose a seller.")]
        [Display(Name = "Purchased From")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        public string Comment { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
