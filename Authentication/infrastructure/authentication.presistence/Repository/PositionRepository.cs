using authentication.application.IRepository;
using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class PositionRepository : IPositionRepository
    {
        public Task<Position> deletePosition(Position position)
        {
            throw new NotImplementedException();
        }

        public Task<Position> getPosition(int positionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> insertPosition(Position position)
        {
            throw new NotImplementedException();
        }

        public Task<Position> updatePosition(Position position)
        {
            throw new NotImplementedException();
        }
    }
}
