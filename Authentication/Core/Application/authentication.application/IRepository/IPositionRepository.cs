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
        Task<Position> updatePosition(Position position);
        Task<Position> deletePosition(Position position);
        Task<bool> insertPosition(Position position);
        Task<Position> getPosition(int  positionId);

    }
}
