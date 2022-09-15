using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class DomainDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public int MembersCount { get; set; }
    }
}
