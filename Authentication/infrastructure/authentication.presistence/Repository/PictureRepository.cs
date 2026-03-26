using authentication.application.IRepository;
using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class PictureRepository : IPictureRepository
    {
        public Task<signitureimage> addimage(signitureimage image)
        {
            throw new NotImplementedException();
        }

        public Task<signitureimage> getimage(int id)
        {
            throw new NotImplementedException();
        }
    }
}
