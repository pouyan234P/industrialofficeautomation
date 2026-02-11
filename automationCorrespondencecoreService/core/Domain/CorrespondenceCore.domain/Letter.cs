using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public enum Priority
    {
        Normal,
        immediate,
        instantaneous
    }
    public enum Confidentiality
    {
        Normal,
        confidential,
        secret
    }
    public enum Type
    {
        Domestic,
        imported,
        exported
    }
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
        public Type type { get; set; }
        public string? BodyHTML { get; set; }


    }
}
