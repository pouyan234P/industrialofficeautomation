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
    public class getReferralRequestHandler : IRequestHandler<getReferralRequest, IEnumerable<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<referralDTO>> Handle(getReferralRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAll();
            var map = _mapper.Map<IEnumerable<referralDTO>>(result);
            return map;
        }
    }
}
