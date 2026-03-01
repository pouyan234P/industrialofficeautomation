using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Model.Authentication
{
    public class registerDTO
    {
        public string? Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string family { get; set; }
        public string Country { get; set; }
        public string phoneNumber { get; set; }
        public string Role { get; set; }
    }
}
