using System;

namespace Vouchers.Application.Dtos;

public sealed record IdDto<TKey>(TKey Id);