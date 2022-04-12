using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Infrastructure
{
    public interface IMapper<T, TResult>
    {
        TResult Map(T entity);
    }
}
