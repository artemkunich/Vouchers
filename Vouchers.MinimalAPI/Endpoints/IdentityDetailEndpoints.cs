using Microsoft.AspNetCore.Mvc;
using Vouchers.MinimalAPI.Filters;
using Vouchers.Application;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MinimalAPI.Binding;

namespace Vouchers.MinimalAPI.Endpoints;

internal static class IdentityDetailEndpoints
{
    internal static void MapIdentityDetailEndpoints(this WebApplication app)
    {
        app.MapGet("identityDetail/{accountId?}", GetIdentityDetail).RequireAuthorization("ApiScope");
        app.MapPost("identityDetail", PostIdentityDetail).RequireAuthorization("ApiScope");
        app.MapPut("identityDetail", PutIdentityDetail).RequireAuthorization("ApiScope");
    }
    
    private static async Task<IResult> GetIdentityDetail(IHandler<Guid?, IdentityDetailDto> handler, Guid? accountId, CancellationToken token)
    {
        var detail = await handler.HandleAsync(accountId, token);
            if (detail is null)
                return Results.NotFound();

            return Results.Ok(detail);
    }
    
    private static async Task<IResult> PostIdentityDetail(IFormParameterProvider<CreateIdentityCommand> formParameterProvider, IHandler<CreateIdentityCommand, Guid> handler, CancellationToken token)
    {
        var command = formParameterProvider.GetParameter();
            
        var identityId = await handler.HandleAsync(command, token);
        return Results.Ok(new { identityId });
    }
    
    private static async Task<IResult> PutIdentityDetail(IFormParameterProvider<UpdateIdentityCommand> formParameterProvider, IHandler<UpdateIdentityCommand> handler, CancellationToken token)
    {
        var command = formParameterProvider.GetParameter();
        
        await handler.HandleAsync(command, token);
        return Results.NoContent();
    }
}