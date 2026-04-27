using CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.request.Queries;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.handles.Queries
{
    public class getlastNumberbyTypeRequestHandler : IRequestHandler<getlastNumberbyTypeRequest, long>
    {
        private readonly IGenerateNextNumberAsyncRepository _repository;

        public getlastNumberbyTypeRequestHandler(IGenerateNextNumberAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> Handle(getlastNumberbyTypeRequest request, CancellationToken cancellationToken)
        {
            var mytype= (Typecorrespondence)request.type;
            var result = await _repository.lastNumber(mytype, request.depid, request.year);
            return result;
        }
    }
}
