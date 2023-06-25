namespace Vouchers.Domains.Application.Queries;

public interface IListQuery
{
    public string OrderBy { get; init; }

    public int PageIndex { get; init; } 

    public int PageSize { get; init; }
}