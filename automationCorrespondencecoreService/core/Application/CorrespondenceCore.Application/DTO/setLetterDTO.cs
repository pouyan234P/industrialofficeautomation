using CorrespondenceCore.Application.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.DTO
{
    public class setLetterDTO
    {
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public int LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public PriorityDTO priority { get; set; }
        public ConfidentialityDTO confidentiality { get; set; }
        public TypeDTO type { get; set; }
        public object? BodyHTML { get; set; }
    }
}
