using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands
{
    public class CreateCollectionCommand
    {
        [Required]
        public Guid CollectionRequestId { get; set; }

        [Required]
        public CreateHolderTransactionCommand CreateHolderTransactionCommand { get; set; }
    }
}
