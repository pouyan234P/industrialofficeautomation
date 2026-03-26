using webapi.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Model.Authentication
{
    public class getuserDTO
    {
        public int id { get; set; }
        public string username { get; set; }
        public int personID { get; set; }
        public bool CurrentStatus { get; set; }
        public getimageidDTO? signitureimageid { get; set; }
    }
}
