using authentication.application.DTO;
using authentication.domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.helper
{
    public class autoMapper:Profile
    {
        public autoMapper() 
        {
            CreateMap<signitureimage, getsignitureimageDTO>().ReverseMap();
            CreateMap<User, getuserDTO>().ForMember(des => des.signitureimageid, mapper => mapper.MapFrom(c => c.SignitureImage));
            CreateMap<signitureimage, getimageidDTO>();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Position, getPosition>().ForMember(dest=>dest.depID,mapper=>mapper.MapFrom(c=>c.Department)).ForPath(dest=>dest.userID.username,mapper=>mapper.MapFrom(c=>c.User.UserName)).ForPath(dest=>dest.userID.id,mapper=>mapper.MapFrom(c=>c.User.Id));

        }
    }
}
