using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IQueryHandler<T> : IRequest<Result<T>>
    {

    }
}
