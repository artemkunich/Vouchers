using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.UseCases
{
    public class UpdateLoginCommand
    {
        [Required]
        public string CurrentLoginName { get; }

        [Required]
        public string NewLoginName { get; }
    }
}
