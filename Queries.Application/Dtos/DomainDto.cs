using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Domains.Application.Dtos;

public sealed class DomainDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsPublic { get; set; }

    public int MembersCount { get; set; }

    public Guid? ImageId { get; set; }
}