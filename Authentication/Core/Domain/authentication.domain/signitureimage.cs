using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.domain
{
    public class signitureimage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } // e.g., "image/jpeg"
        public byte[]? ImageData { get; set; }
    }
}
