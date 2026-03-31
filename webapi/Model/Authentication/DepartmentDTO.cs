using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Model.Authentication
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public int parentID { get; set; }
    }
}
