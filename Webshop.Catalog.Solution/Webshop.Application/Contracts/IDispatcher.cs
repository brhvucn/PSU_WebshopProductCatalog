using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IDispatcher
    {
        Task<Result<T>> Dispatch<T>(IQueryHandler<T> query);
        Task<Result> Dispatch(ICommand command);
    }
}
