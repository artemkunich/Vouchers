using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public class HolderTransactionRequestQuery : IRequest<HolderTransactionRequestDto>
{
    [Required]
    public Guid Id { get; init; }
}