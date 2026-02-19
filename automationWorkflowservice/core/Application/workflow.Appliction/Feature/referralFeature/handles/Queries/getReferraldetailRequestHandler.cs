using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Queries;
using workflow.Appliction.IRepository;

namespace workflow.Appliction.Feature.referralFeature.handles.Queries
{
    public class getReferraldetailRequestHandler : IRequestHandler<getReferraldetailRequest, referralDTO>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferraldetailRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<referralDTO> Handle(getReferraldetailRequest request, CancellationToken cancellationToken)
        {
            var result =await _repository.Get(request.id);
            var map=_mapper.Map<referralDTO>(result);
            return map;
        }
    }
}
