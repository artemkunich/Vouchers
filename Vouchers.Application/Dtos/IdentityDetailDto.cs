using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public sealed class IdentityDetailDto
    {
        public Guid IdentityId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid? ImageId { get; set; }

        public Guid? CroppedImageId { get; set; }

        public CropParametersDto CropParameters { get; set; }

        public Guid? AccountId { get; set; }

        public bool? IsIssuer { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsOwner { get; set; }

    }
}
