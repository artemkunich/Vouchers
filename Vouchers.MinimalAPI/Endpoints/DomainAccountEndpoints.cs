using Microsoft.AspNetCore.Mvc;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.Queries;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class DomainAccountEndpoints
{
    internal static void MapDomainAccountEndpoints(this WebApplication app)
    {
        app.MapGet("domainAccounts", GetDomainAccounts).RequireAuthorization("ApiScope", "RoleUser");
    }
    
    private static async Task<IResult> GetDomainAccounts(IRequestHandler<DomainAccountsQuery,IEnumerable<DomainAccountDto>> requestHandler, 
        [FromQuery] Guid domainId,
        [FromQuery] string? email,
        [FromQuery] string? name,
        [FromQuery] bool? includeConfirmed,
        [FromQuery] bool? includeNotConfirmed,
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        CancellationToken token)
    {

        var query = new DomainAccountsQuery()
        {
            DomainId = domainId,
            Email = email,
            Name = name,
            IncludeConfirmed = includeConfirmed ?? true,
            IncludeNotConfirmed = includeNotConfirmed ?? false,
            PageIndex = pageIndex ?? 0,
            PageSize = pageSize ?? 10,
        };
        var result = await requestHandler.HandleAsync(query, token);
        return Results.Ok(result);
    }
}