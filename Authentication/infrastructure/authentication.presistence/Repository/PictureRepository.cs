using authentication.application.IRepository;
using authentication.domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class PictureRepository : IPictureRepository
    {
        private readonly authDb _db;

        public PictureRepository(authDb db)
        {
            _db = db;
        }
        public async Task<signitureimage> addimage(signitureimage image)
        {
            _db.signitureimages.Add(image);
            _db.SaveChanges();
            var myimage = await _db.signitureimages.Select(x => x).OrderBy(x => x).LastOrDefaultAsync();
            return myimage;
        }

        public async Task<signitureimage> getimage(int id)
        {
            var image=await _db.signitureimages.Where(x=>x.Id==id).Select(x=>x).FirstOrDefaultAsync();
            return image;
        }
    }
}
