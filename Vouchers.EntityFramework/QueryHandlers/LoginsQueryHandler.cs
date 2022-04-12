using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;
using System.Linq;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities;
using Vouchers.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Domains;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class LoginsQueryHandler : IHandler<LoginsQuery, IPaginatedEnumerable<LoginDto>>
    {
        VouchersDbContext dbContext;

        public LoginsQueryHandler(VouchersDbContext dbContext)
        {           
            this.dbContext = dbContext;
        }

        public async Task<IPaginatedEnumerable<LoginDto>> HandleAsync(LoginsQuery query, CancellationToken cancellation) =>
            await PaginatedList<LoginDto>.CreateAsync(GetQuery(query), query.PageIndex, query.PageSize);


        private IQueryable<LoginDto> GetQuery(LoginsQuery query)
        {
            var loginsQuery = dbContext.Logins
                .Include(login => login.Identity)
                .AsQueryable();

            if (query.LoginName != null)
                loginsQuery = loginsQuery.Where(login => login.LoginName.Contains(query.LoginName));


            var identityDetailsQuery = dbContext.IdentityDetails
                .Include(detail => detail.Identity)
                .AsQueryable();

            if (query.IdentityName != null)
                identityDetailsQuery = identityDetailsQuery.Where(detail => detail.IdentityName.Contains(query.IdentityName));

            if (query.FirstName != null)
                identityDetailsQuery = identityDetailsQuery.Where(detail => detail.FirstName.Contains(query.FirstName));

            if (query.LastName != null)
                identityDetailsQuery = identityDetailsQuery.Where(detail => detail.LastName.Contains(query.LastName));


            var resultQuery = loginsQuery
                .Join(identityDetailsQuery,
                    login => login.Identity.Id,
                    detail => detail.Identity.Id,
                    (login, detail) => new LoginDto
                    {
                        Id = login.Id,
                        LoginName = login.LoginName,
                        IdentityName = detail.IdentityName,
                        FirstName = detail.FirstName,
                        LastName = detail.LastName
                    }
                );

            switch (query.OrderBy)
            {
                case "LoginName":
                    resultQuery = resultQuery.OrderBy(login => login.LoginName);
                    break;
                case "LoginNameDesc":
                    resultQuery = resultQuery.OrderByDescending(login => login.LoginName);
                    break;
                case "IdentityName":
                    resultQuery = resultQuery.OrderBy(login => login.IdentityName);
                    break;
                case "IdentityNameDesc":
                    resultQuery = resultQuery.OrderByDescending(login => login.IdentityName);
                    break;
                case "FirstName":
                    resultQuery = resultQuery.OrderBy(login => login.FirstName);
                    break;
                case "FirstNameDesc":
                    resultQuery = resultQuery.OrderByDescending(login => login.FirstName);
                    break;
                case "LastName":
                    resultQuery = resultQuery.OrderBy(login => login.LastName);
                    break;
                case "LastNameDesc":
                    resultQuery = resultQuery.OrderByDescending(login => login.LastName);
                    break;
            }

            resultQuery.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize);

            return resultQuery;
        }
    }
}
