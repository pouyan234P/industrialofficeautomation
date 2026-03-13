using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Responses
{
    public class baseCommandResponse
    {
        public int id { get; set; }
        public bool Success { get; set; } = true;
        public object Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
