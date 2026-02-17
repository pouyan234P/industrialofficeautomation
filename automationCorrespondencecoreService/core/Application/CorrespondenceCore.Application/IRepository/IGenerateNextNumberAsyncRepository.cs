using CorrespondenceCore.Application.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.IRepository
{
    public interface IGenerateNextNumberAsyncRepository
    {
        Task<string> mynextnumber(TypeDTO type, int year, int? deptId = 0);
    }
}
