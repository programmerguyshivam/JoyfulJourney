using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.DTOs
{
    public class AddAdminDestinationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string ImageURl { get; set; }
        public IFormFile  IMAGE { get; set; }
    }
}