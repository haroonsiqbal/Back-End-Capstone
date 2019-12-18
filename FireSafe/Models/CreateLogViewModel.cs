using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireSafe.Models
{
    public class CreateLogViewModel
    {
        public Log Log { get; set; }
        public IFormFile MyImage { get; set; }

    }
}
