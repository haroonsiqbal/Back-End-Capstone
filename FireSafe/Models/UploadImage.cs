using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FireSafe.Models
{
    public class UploadImage
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageFile { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
