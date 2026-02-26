using AutoMapper;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.Feature.referralFeature.request.Command;
using workflow.Appliction.IRepository;
using workflow.Appliction.Responses;
using workflow.domain;

namespace workflow.Appliction.Feature.referralFeature.handles.Command
{
    public class createReferralCommandHandler : IRequestHandler<createReferralCommand, baseCommandResponse>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public createReferralCommandHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<baseCommandResponse> Handle(createReferralCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var map = _mapper.Map<Referral>(request.setReferral);
            var result = _repository.Add(map.ToBsonDocument());
            if (result != null)
            {
                response.Success=true;
                response.Message = result.Result.ToString()!;
            }
            return response;

        }
    }
}
