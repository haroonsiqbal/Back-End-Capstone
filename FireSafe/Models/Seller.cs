using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireSafe.Models
{
    public class Seller
    {
        [Key]
        public int SellerId { get; set; }
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
