﻿using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;

namespace Vouchers.Application.Commands.HolderTransactionRequestCommands;

public sealed class DeleteHolderTransactionRequestCommand : IRequest<Unit>
{
    [Required]
    public Guid HolderTransactionRequestId { get; set; }
}