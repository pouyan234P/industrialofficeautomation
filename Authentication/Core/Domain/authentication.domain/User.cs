
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.domain
{
    public class User: IdentityUser<int>
    {
        public virtual ICollection<UserRole> userRoles { get; set; }
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public string Name { get; set; }
        public string family { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        // --- THE FIX IS HERE ---

        // 1. The actual Integer Foreign Key (Nullable so it doesn't crash if a user has no image)
        public int? SignitureImageId { get; set; }

        // 2. The Navigation Property (The Object)
        public signitureimage? SignitureImage { get; set; }
    }
}
