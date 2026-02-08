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
        public Department depID { get; set; }
        public User userID { get; set; }
    }
}
