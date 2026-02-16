using CorrespondenceCore.domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public class Letter
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public int LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Priority priority { get; set; }
        public Confidentiality confidentiality { get; set; }
        public Types type { get; set; }
        public string? BodyHTMLID { get; set; }
    }
}
