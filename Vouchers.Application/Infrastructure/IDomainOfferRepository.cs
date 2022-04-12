using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.Application.Infrastructure
{
    public interface IDomainOfferRepository
    {
        Task<DomainOffer> GetByIdAsync();
        DomainOffer GetById();

        Task AddAsync(DomainOffer domainOffer);
        void Add(DomainOffer domainOffer);

        void Update(DomainOffer domainOffer);

        Task SaveAsync();
        void Save();
    }
}
