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

        public createAuthCommandHandler(IMapper mapper, IAuthformRepository authformRepository)
        {
            _mapper = mapper;
            _authformRepository = authformRepository;
        }

        public async Task<baseCommandResponse> Handle(createAuthCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var dto = request.registerdto!;
            var mypicture = _mapper.Map<signitureimage>(dto.signitureid);
            var myuser = new User
            {
                Email = dto.Email!.Trim(),
                family = dto.family!.Trim(),
                Name = dto.Name!.Trim(),
                PhoneNumber = dto.phoneNumber,
                Country = dto.Country,
                Date = DateTime.Now.Date,
                // An explicit username wins; otherwise fall back to the old "Name_family" convention.
                UserName = string.IsNullOrWhiteSpace(dto.Username)
                    ? dto.Name!.Trim() + "_" + dto.family!.Trim()
                    : dto.Username.Trim(),
                CurrentStatus = dto.CurrentStatus ?? true,
                SignitureImage = mypicture,
                personID = dto.personID
            };

            var errors = await _authformRepository.regiseter(myuser, dto.Password!, dto.Role!);
            if (errors.Count == 0)
            {
                response.Success = true;
                response.Message = "Creation was Successful";
                response.id = myuser.Id;
            }
            else
            {
                response.Success = false;
                response.Message = "User creation failed";
                response.Errors = errors;
            }
            return response;
        }
    }
}
