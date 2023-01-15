using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Infrastructure;

public interface IMapper<in T, out TResult>
{
    TResult Map(T entity);
}