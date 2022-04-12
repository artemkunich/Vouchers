using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Queries
{
    public interface IPaginatedEnumerable<T> : IEnumerable<T>
    {
        int PageIndex { get; }
        int TotalPages { get; }
    }
}
