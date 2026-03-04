using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.DTO
{
    public class setPosition
    {
        public string Title { get; set; }
        // 1. The Foreign Keys (The actual integers saved in the Position table)
        public int departmentId { get; set; }
        public int userId { get; set; }
    }
}
