using searchengine.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Application.IRepository
{
    public interface ILetterSearchRepository
    {
        Task<bool> IndexLetterAsync(LetterSearchDocument document);
        Task<IEnumerable<LetterSearchDocument>> SearchAsync(string keyword, DateTime? fromDate, DateTime? toDate);
    }
}
