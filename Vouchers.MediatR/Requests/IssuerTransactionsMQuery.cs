using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;

namespace Vouchers.MediatR.Requests 
{
    public class IssuerTransactionsMQuery : AuthorizedRequest<IEnumerable<IssuerTransactionDto>>
    {
        public IssuerTransactionsQuery AppQuery { get; set;}
    }
}
