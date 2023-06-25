using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class DeleteImageCommand : IRequest<Unit>
{
    [Required]
    public Guid ImageId { get; set; }
}