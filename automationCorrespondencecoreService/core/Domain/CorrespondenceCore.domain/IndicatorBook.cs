using CorrespondenceCore.domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public class IndicatorBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FiscalYear { get; set; } // Example: 1403

        [Required]
        public Typecorrespondence? type { get; set; } // Enum: Internal, Incoming, Outgoing

        [Required]
        public long LastNumber { get; set; } // The counter (e.g., 5001)

        [MaxLength(20)]
        public string? Prefix { get; set; } // Optional: "INT", "EXT", "A"

        [MaxLength(20)]
        public string? Separator { get; set; } // Optional: "/", "-"

        // Concurrency Token: Crucial for high-traffic systems
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public int DepartmentID { get; set; }
    }
}
