using authentication.application.Feature.positionRepository.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using authentication.domain;
using MediatR;

namespace authentication.application.Feature.positionRepository.handle.Commands
{
    public class updatePositionCommandHandler : IRequestHandler<updatePositionCommand, baseCommandResponse>
    {
        private readonly IPositionRepository _repository;

        public updatePositionCommandHandler(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task<baseCommandResponse> Handle(updatePositionCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var changes = new Position
            {
                Title = request.positionDTO!.Title,
                departmentId = request.positionDTO.departmentId,
                userId = request.positionDTO.userId
            };

            var result = await _repository.updatePosition(request.id, changes);
            if (result != null)
            {
                response.Success = true;
                response.Message = "Position updated successfully";
                response.id = result.Id;
            }
            else
            {
                response.Success = false;
                response.Message = "Position update failed";
                response.Errors = new List<string>() { $"Position {request.id} was not found." };
            }
            return response;
        }
    }
}
