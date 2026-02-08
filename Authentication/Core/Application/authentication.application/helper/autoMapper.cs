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
            CreateMap<Department, DepartmentDTO>();
            CreateMap<Position, getPosition>();
        }
    }
}
