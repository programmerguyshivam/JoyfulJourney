using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.DTOs
{
    public class GetBookUserDTO
    {

    public class BookNowRead
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int BookID { get; set; }

            [StringLength(255)]
            public string Email { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime? Date { get; set; }

            [StringLength(100)]
            public string Destination { get; set; }

            [Range(1, 8)]
            public byte? Persons { get; set; }

            [StringLength(20)]
            public string Category { get; set; }

            [StringLength(int.MaxValue)] // or specify a length if needed
            public string SpecialRequest { get; set; }
        }

    }
}
