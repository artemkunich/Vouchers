using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class IdentityDetailDto
    {
        public Guid? IdentityId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public IFormFile Image { get; set; }

        public string ImageBase64 { get; set; }

        public string CroppedImageBase64 { get; set; }

        public CropParametersDto CropParameters { get; set; }

    }
}
