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
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public string Name { get; set; }
        public string family { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
    }
}
