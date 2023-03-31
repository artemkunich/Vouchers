using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public class HolderTransactionRequestQuery : IRequest<HolderTransactionRequestDto>
{
    [Required]
    public Guid Id { get; init; }
}