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
        public Department depID { get; set; }
        public User userID { get; set; }
    }
}
