using Microsoft.AspNetCore.Mvc;
using Vouchers.MinimalAPI.Filters;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
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
    
    private static async Task<IResult> GetIdentityDetail(IRequestHandler<IdentityDetailQuery, IdentityDetailDto> requestHandler, Guid? accountId, CancellationToken token)
    {
        var detail = await requestHandler.HandleAsync(new IdentityDetailQuery() {AccountId = accountId}, token);
            if (detail is null)
                return Results.NotFound();

            return Results.Ok(detail);
    }
    
    private static async Task<IResult> PostIdentityDetail(IFormParameterProvider<CreateIdentityCommand> formParameterProvider, IRequestHandler<CreateIdentityCommand, IdDto<Guid>> requestHandler, CancellationToken token)
    {
        var command = formParameterProvider.GetParameter();
            
        var identityId = await requestHandler.HandleAsync(command, token);
        return Results.Ok(new { identityId });
    }
    
    private static async Task<IResult> PutIdentityDetail(IFormParameterProvider<UpdateIdentityCommand> formParameterProvider, IRequestHandler<UpdateIdentityCommand,Unit> requestHandler, CancellationToken token)
    {
        var command = formParameterProvider.GetParameter();
        
        await requestHandler.HandleAsync(command, token);
        return Results.NoContent();
    }
}