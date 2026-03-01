using webapi.Model.CorrespondenceModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Model.CorrespondenceModel
{
    public class AttachmentDTO
    {
        public int id { get; set; }
        public LetterDTO LetterID { get; set; }
        public string? FilePath { get; set; }
        public FileType MyProperty { get; set; }
    }
}
