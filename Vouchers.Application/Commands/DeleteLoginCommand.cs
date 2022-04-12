﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.UseCases
{
    public class DeleteLoginCommand
    {
        [Required]
        public string LoginName { get; }
    }
}
