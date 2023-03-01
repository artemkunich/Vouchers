using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.IdentityCommands;

public sealed class DeleteIdentityCommand : IRequest<Unit>
{
}