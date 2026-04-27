using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.domain;
using CorrespondenceCore.domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.IRepository
{
    public interface ILetterRepository:IGenericRepository<Letter>
    {
        Task<IEnumerable<Letter>> GetLetterbyTypeAsync(Typecorrespondence type);
    }
}
