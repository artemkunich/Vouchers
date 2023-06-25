using Akunich.Application.Abstractions;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand,Unit>
{
    public Task<Result<Unit>> HandleAsync(DeleteImageCommand request, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}