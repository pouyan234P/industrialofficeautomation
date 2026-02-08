using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using authentication.domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.handles.Commands
{
    
    public class createAuthCommandHandler : IRequestHandler<createAuthCommand, baseCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAuthformRepository _authformRepository;

        public createAuthCommandHandler(IMapper mapper,IAuthformRepository authformRepository)
        {
            _mapper = mapper;
            _authformRepository = authformRepository;
        }
        public async Task<baseCommandResponse> Handle(createAuthCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var myuser = new User
            {
                Email = request.registerDTO.Email,
                family = request.registerDTO.family,
                Name = request.registerDTO.Name,
                PhoneNumber = request.registerDTO.phoneNumber,
                Country = request.registerDTO.Country,
                Date = DateTime.Now.Date,
            };
            var myusersend = await _authformRepository.regiseter(myuser, request.registerDTO.Password);
            if (myusersend != null)
            {
                response.Success = true;
                response.Message = "Creation was Successful";
                response.id=myuser.Id;
            }
            return response;
        }
    }
}
