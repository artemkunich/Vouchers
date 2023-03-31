using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Permissions;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

[IdentityPermission]
public sealed class DeleteIdentityCommand : IRequest<Unit>
{
}