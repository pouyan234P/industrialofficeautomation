using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.IRepository
{
    public interface IPictureRepository
    {
        Task<signitureimage> addimage(signitureimage image);
        Task<signitureimage> getimage(int id);
    }
}
