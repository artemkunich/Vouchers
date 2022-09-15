using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;

namespace Vouchers.MediatR.Requests
{
    public class HolderTransactionsMQuery : AuthorizedRequest<IEnumerable<HolderTransactionDto>>
    {
        public HolderTransactionsQuery AppQuery { get; set; }
    }
}
