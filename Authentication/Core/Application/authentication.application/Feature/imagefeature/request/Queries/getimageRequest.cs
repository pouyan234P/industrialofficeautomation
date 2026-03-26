using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.imagefeature.request.Queries
{
    public class getimageRequest:IRequest<getsignitureimageDTO>
    {
        public int id { get; set; }
    }
}
