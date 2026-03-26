using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.imagefeature.request.Commands
{
    public class createimageCommand : IRequest<getsignitureimageDTO>
    {
        public signitureimageDTO signitureimageDTO { get; set; }
    }
}
