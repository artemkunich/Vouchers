using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;

namespace Vouchers.MVC.Models
{
    public class DomainOffersViewModel
    {
        public string OrderBy { get; set; }
        public string LoginNameOrder { get; set; }
        public string IdentityNameOrder { get; set; }
        public string FirstNameOrder { get; set; }
        public string LastNameOrder { get; set; }

        public string LoginNameFilter { get; set; }
        public string IdentityNameFilter { get; set; }
        public string FirstNameFilter { get; set; }
        public string LastNameFilter { get; set; }

        public IPaginatedEnumerable<DomainOfferDto> Items { get; set; }
    }
}
