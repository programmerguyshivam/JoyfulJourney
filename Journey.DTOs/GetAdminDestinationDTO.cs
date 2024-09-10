using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.DTOs
{
    public  class GetAdminDestinationDTO
    {
        public int DestinationId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string ImageURL { get; set; }

    }
}
