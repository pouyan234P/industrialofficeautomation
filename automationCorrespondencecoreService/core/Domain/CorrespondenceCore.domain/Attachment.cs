using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public enum FileType
    {
        PDF,
        JPG,
        DOCX
    }
    public class Attachment
    {
        public int ID { get; set; }
        public Letter? LetterID { get; set; }
        public string? FilePath { get; set; }
        public FileType fileType { get; set; }
    }
}
