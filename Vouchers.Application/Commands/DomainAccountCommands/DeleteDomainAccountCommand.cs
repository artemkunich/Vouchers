using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainAccountCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DeleteDomainAccountCommand : IRequest<Unit>
{      
}