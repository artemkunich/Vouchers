using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Domains.Application.Queries;

public abstract class ListQuery : IListQuery
{
    public string OrderBy { get; init; }

    public int PageIndex { get; init; } = 0;

    public int PageSize { get; init; } = 10;
}