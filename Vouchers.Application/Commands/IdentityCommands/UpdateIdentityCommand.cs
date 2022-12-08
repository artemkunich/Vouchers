using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.IdentityCommands
{
    public sealed class UpdateIdentityCommand
    {
        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IFormFile Image { get; set; }

        public CropParametersDto CropParameters { get; set; }

    }
}
