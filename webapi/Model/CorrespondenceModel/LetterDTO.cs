
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Model.CorrespondenceModel
{
    public class LetterDTO
    {
        public int id { get; set; }
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public string LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public PriorityDTO MyProperty { get; set; }
        public ConfidentialityDTO confidentiality { get; set; }
        public TypeDTO type { get; set; }
        public string? BodyHTML { get; set; }
    }
}
