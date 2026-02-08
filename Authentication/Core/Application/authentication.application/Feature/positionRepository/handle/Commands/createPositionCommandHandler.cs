using authentication.application.Feature.positionRepository.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using authentication.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.handle.Commands
{
    public class createPositionCommandHandler : IRequestHandler<createPositionCommand, baseCommandResponse>
    {
        private readonly IPositionRepository _repository;

        public createPositionCommandHandler(IPositionRepository repository)
        {
            _repository = repository;
        }
        public async Task<baseCommandResponse> Handle(createPositionCommand request, CancellationToken cancellationToken)
        {
            var response=new baseCommandResponse();
            var myposition = new Position
            {
                depID = request.positionDTO.depID,
                Title = request.positionDTO.Title,
                userID = request.positionDTO.userID
            };
            var result = await _repository.insertPosition(myposition);
            if (result == true)
            {
                response.Success = true;
                response.Message = "Created succefully";
            }
            else {
                string message = "something went wrong";
                response.Success = false;
                response.Errors = new List<string>();
                response.Errors.Add(message);

            }
            return response;
        }
    }
}
