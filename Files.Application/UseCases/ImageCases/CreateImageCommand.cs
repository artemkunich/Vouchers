using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Files.Application.Dtos;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class CreateImageCommand : IRequest<Guid>, IRequest<IdDto<Guid>>
{
    [Required]
    public Guid SubjectId { get; set; }

    [Required]
    public IFormFile Image { get; set; }

    [Required]
    public CropParametersDto CropParameters { get; set; }
}