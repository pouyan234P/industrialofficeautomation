using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.domain
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        // 1. The Foreign Keys (The actual integers saved in the Position table)
        public int departmentId { get; set; }
        public int userId { get; set; }

        // 2. The Navigation Properties (The objects EF Core uses to join tables)
        public Department Department { get; set; }
        public User User { get; set; }
    }
}
