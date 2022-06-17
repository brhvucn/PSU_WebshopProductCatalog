using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult>
    {
        new Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}
