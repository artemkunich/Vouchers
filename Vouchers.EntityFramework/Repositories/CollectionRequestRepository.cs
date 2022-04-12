using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    public class CollectionRequestRepository : ICollectionRequestRepository
    {
        public async Task<CollectionRequest> GetByIdAsync(Guid collectionRequestId)
        {
            throw new NotImplementedException();
        }

        public CollectionRequest GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(CollectionRequest collection)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(CollectionRequest collectionRequest)
        {
            throw new NotImplementedException();
        }

        public void Update(CollectionRequest collection)
        {
            throw new NotImplementedException();
        } 

        public void Save()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
