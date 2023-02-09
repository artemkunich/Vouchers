using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries;

public interface IListQuery
{
    public string OrderBy { get; init; }

    public int PageIndex { get; init; } 

    public int PageSize { get; init; }
}