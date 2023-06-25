using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.HolderTransactionRequestCases;

public sealed class DeleteHolderTransactionRequestCommand : IRequest<Unit>
{
    [Required]
    public Guid HolderTransactionRequestId { get; set; }
}