using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.IRepository
{
    public interface IPositionRepository
    {
        Task<Position> updatePosition(int id,Position position);
        Task<bool> deletePosition(int id);
        Task<bool> insertPosition(Position position);
        Task<Position> getPositionbyuser(int  userid);
        Task<IEnumerable<Position>> getAll();
        Task<Position> getPosdep(int deptid,int posid);
        Task<Position> getPosition(int posid);
        Task<Position> getPositionbyDept(int deptid);
    }
}
