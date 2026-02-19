using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workflow.Persistence.helper
{
    public class Mongosettings
    {
        public required string Connection { get; set; }
        public required string DatabaseName { get; set; }
    }
}
