using Microsoft.AspNetCore.Mvc;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MinimalAPI.Filters;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class DomainAccountEndpoints
{
    internal static void MapDomainAccountEndpoints(this WebApplication app)
    {
        app.MapGet("domainAccounts", GetDomainAccounts).RequireAuthorization("ApiScope", "RoleUser");
    }
    
    private static async Task<IResult> GetDomainAccounts(IHandler<DomainAccountsQuery,IEnumerable<DomainAccountDto>> handler, 
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
        var result = await handler.HandleAsync(query, token);
        return Results.Ok(result);
    }
}