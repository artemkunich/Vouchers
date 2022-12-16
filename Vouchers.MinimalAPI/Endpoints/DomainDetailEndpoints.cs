using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MinimalAPI.Binding;
using Vouchers.MinimalAPI.Filters;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class DomainDetailEndpoints
{
    internal static void MapDomainDetailEndpoints(this WebApplication app)
    {
        app.MapGet("domainDetail/{domainId:guid}", GetDomainDetail).RequireAuthorization("ApiScope", "RoleUser");
        app.MapPut("domainDetail", PutDomainDetail).RequireAuthorization("ApiScope", "RoleUser").AddEndpointFilter<FormValidatorFilter<UpdateDomainDetailCommand>>();
    }

    private static async Task<IResult> GetDomainDetail(IHandler<Guid,DomainDetailDto> handler, Guid domainId, CancellationToken token)
    {
        var result = await handler.HandleAsync(domainId, token);
        return Results.Ok(result);
    }
    
    private static async Task<IResult> PutDomainDetail(IFormParameterProvider<UpdateDomainDetailCommand> formParameterProvider, IHandler<UpdateDomainDetailCommand> handler, HttpContext ctx, CancellationToken token)
    {
        var command = formParameterProvider.GetParameter();
        await handler.HandleAsync(command, token);
        return Results.NoContent();
    }
}