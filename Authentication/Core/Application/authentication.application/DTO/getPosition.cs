using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.DTO
{
    public class getPosition
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public Department? depID { get; set; }
        public getuserDTO? userID { get; set; }
    }
}
