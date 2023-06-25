using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.Queries;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.UseCases.IdentityCases;
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
    
    private static async Task<IResult> PostIdentityDetail(IFormParameterProvider<CreateIdentityCommand> formParameterProvider, IRequestHandler<CreateIdentityCommand, Common.Application.Dtos.IdDto<Guid>> requestHandler, CancellationToken token)
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