using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireSafe.Models
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        [Display(Name = "Policy No.")]
        public int PolicyNumber { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Amount Covered")]
        public double CoverageAmount { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Riders { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
