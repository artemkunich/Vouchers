using Microsoft.AspNetCore.Mvc;
using Vouchers.MinimalAPI.Filters;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class IdentityDomainAccountEndpoints
{
    internal static void MapIdentityDomainAccountEndpoints(this WebApplication app)
    {
        app.MapGet("identityDomainAccounts", GetDomainAccounts).RequireAuthorization("ApiScope", "RoleUser");
    }
    
    private static async Task<IResult> GetDomainAccounts(IHandler<IdentityDomainAccountsQuery,IEnumerable<DomainAccountDto>> handler,
        [FromQuery] string? domainName,
        CancellationToken token)
    {

        var query = new IdentityDomainAccountsQuery()
        {
            DomainName = domainName
        };
        var result = await handler.HandleAsync(query, token);
        return Results.Ok(result);
    }
}