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
        Task<Position> getPosition(int  positionId);
        Task<IEnumerable<Position>> getAll();

    }
}
