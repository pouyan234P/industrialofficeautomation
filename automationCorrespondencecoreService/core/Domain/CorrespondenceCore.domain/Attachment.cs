using CorrespondenceCore.domain.Enum;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public class Attachment
    {
        public int ID { get; set; }
        public Letter? LetterID { get; set; }
        public string? FilePath { get; set; }
        public FileType MyProperty { get; set; }
    }
}
