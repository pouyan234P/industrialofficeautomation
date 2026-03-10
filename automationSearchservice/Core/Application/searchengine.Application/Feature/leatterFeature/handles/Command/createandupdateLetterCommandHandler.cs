using MediatR;
using searchengine.Application.Feature.leatterFeature.request.Command;
using searchengine.Application.IRepository;
using searchengine.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Application.Feature.leatterFeature.handles.Command
{
    public class createandupdateLetterCommandHandler : IRequestHandler<createandupdateLetterCommand, baseCommandResponse>
    {
        private readonly ILetterSearchRepository _repository;

        public createandupdateLetterCommandHandler(ILetterSearchRepository repository)
        {
            _repository = repository;
        }
        public async Task<baseCommandResponse> Handle(createandupdateLetterCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            try
            {
                
                var result = await _repository.IndexLetterAsync(request.document!);
                if (result == true)
                {
                    response.Success = true;
                    response.Message = result.ToString();
                }
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.ToString();
            }
            return response;
        }
    }
}
