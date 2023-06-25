using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Files.Application.Dtos;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class UpdateCropParametersCommand : IRequest<Unit>
{
    [Required]
    public Guid ImageId { get; set; }
    
    [Required]
    public CropParametersDto CropParameters { get; set; }
}