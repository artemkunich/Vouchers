using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class DomainAccountDto
    {
        public Guid Id { get; set; }

        public Guid DomainId { get; set; }

        public string DomainName { get; set; }


        public Guid IdentityId { get; set; } 

        public string Email { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsIssuer { get; set; }

        public bool IsOwner { get; set; }

        public bool IsConfirmed { get; set; }

        public string ImageBase64 { get; set; }
    }
}
