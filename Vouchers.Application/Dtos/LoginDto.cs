using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos;

public sealed class LoginDto
{
    public Guid Id { get; set; }

    public string LoginName { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}