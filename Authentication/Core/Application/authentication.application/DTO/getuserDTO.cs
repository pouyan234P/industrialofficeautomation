using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.DTO
{
    public class getuserDTO
    {
        public int id { get; set; }
        public string username { get; set; }
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public getimageidDTO? signitureimageid { get; set; }
        /// <summary>Names of the roles this user holds (filled by the repository, not AutoMapper).</summary>
        public List<string> roles { get; set; } = new();
    }
}
