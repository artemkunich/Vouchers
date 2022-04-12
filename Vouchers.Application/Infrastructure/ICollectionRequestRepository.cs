using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface ICollectionRequestRepository
    {
        Task<CollectionRequest> GetByIdAsync(Guid collectionRequestId);
        CollectionRequest GetById(Guid collectionRequestId);

        Task AddAsync(CollectionRequest collectionRequest);
        void Add(CollectionRequest collectionRequest);

        void Update(CollectionRequest collectionRequest);

        Task SaveAsync();
        void Save();
    }
}
