using Microsoft.AspNetCore.Mvc;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.Queries;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class IdentityDomainAccountEndpoints
{
    internal static void MapIdentityDomainAccountEndpoints(this WebApplication app)
    {
        app.MapGet("identityDomainAccounts", GetDomainAccounts).RequireAuthorization("ApiScope", "RoleUser");
    }
    
    private static async Task<IResult> GetDomainAccounts(IRequestHandler<IdentityDomainAccountsQuery,IEnumerable<DomainAccountDto>> requestHandler,
        [FromQuery] string? domainName,
        CancellationToken token)
    {

        var query = new IdentityDomainAccountsQuery()
        {
            DomainName = domainName
        };
        var result = await requestHandler.HandleAsync(query, token);
        return Results.Ok(result);
    }
}