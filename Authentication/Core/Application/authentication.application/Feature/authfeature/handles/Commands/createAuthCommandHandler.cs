using authentication.application.DTO;
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
            var mypicture=_mapper.Map<signitureimage>(request.registerdto.signitureid);
            var myuser = new User
            {
                Email = request.registerdto.Email,
                family = request.registerdto.family,
                Name = request.registerdto.Name,
                PhoneNumber = request.registerdto.phoneNumber,
                Country = request.registerdto.Country,
                Date = DateTime.Now.Date,
                UserName=request.registerdto.Name+"_"+request.registerdto.family,
                SignitureImage = mypicture,
                personID=request.registerdto.personID
            };
            var myusersend = await _authformRepository.regiseter(myuser, request.registerdto.Password,request.registerdto.Role);
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
