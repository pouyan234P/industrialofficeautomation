using CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.request;
using CorrespondenceCore.Application.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.handles
{
    public class generateNextNumberRequesthandler : IRequestHandler<generateNextNumberRequest, string>
    {
        private readonly IGenerateNextNumberAsyncRepository _repository;

        public generateNextNumberRequesthandler(IGenerateNextNumberAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> Handle(generateNextNumberRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.mynextnumber(request.type, request.year, request.depid);
            return result;
        }
    }
}
