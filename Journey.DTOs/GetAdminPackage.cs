using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.DTOs
{
    public  class GetAdminPackage
    {
        public int PackageId { get; set; }  
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }
        public int Destination_id { get; set; }
        public string ImageURL { get; set; }

    }
}
