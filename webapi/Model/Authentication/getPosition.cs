
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Model.Authentication
{
    public class getPosition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DepartmentDTO depID { get; set; }
        public getuserDTO userID { get; set; }
    }
}
