using AutoMapper;
using MediatR;
using MongoDB.Bson.Serialization;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Queries;
using workflow.Appliction.IRepository;
using workflow.domain;

namespace workflow.Appliction.Feature.referralFeature.handles.Queries
{
    public class getReferralbyreciverRequestHandler : IRequestHandler<getReferralbyreciverRequest, PagedList<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralbyreciverRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PagedList<referralDTO>> Handle(getReferralbyreciverRequest request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetAllbyreciverposition(request.id, request.userParams);

            // 2. Deserialize and Map
            var myref = items.Select(doc => BsonSerializer.Deserialize<Referral>(doc));
            var dtoItems = _mapper.Map<IEnumerable<referralDTO>>(myref);

            // 3. Instantiate the PagedList exactly ONCE with the mapped items
            var pagedDtoList = new PagedList<referralDTO>(
                dtoItems,
                totalCount,
                request.userParams.PageNumber,
                request.userParams.pageSize
            );

            return pagedDtoList;
        }
    }
}
