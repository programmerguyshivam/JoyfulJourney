using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.DTOs
{
    public class AddBookUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateAndTime { get; set; } 
        public string Destinations { get; set; }
        public string Persons { get; set; } 
        public string Categories { get; set; }
        public string SpecialRequest { get; set; }
    }
}
