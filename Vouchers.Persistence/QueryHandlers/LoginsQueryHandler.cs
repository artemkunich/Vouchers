using System.Collections.Generic;
using System.Linq;
using Vouchers.Identities.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;
using Vouchers.Application;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class LoginsQueryHandler : IHandler<LoginsQuery, IEnumerable<LoginDto>>
{
    VouchersDbContext _dbContext;

    public LoginsQueryHandler(VouchersDbContext dbContext)
    {           
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<LoginDto>>> HandleAsync(LoginsQuery query, CancellationToken cancellation) =>
        await GetQuery(query).ToListAsync(cancellation);


    private IQueryable<LoginDto> GetQuery(LoginsQuery query)
    {
        var loginsQuery = _dbContext.Set<Login>()
            .Include(login => login.Identity)
            .AsQueryable();

        if (query.LoginName != null)
            loginsQuery = loginsQuery.Where(login => login.LoginName.Contains(query.LoginName));


        var identitiesQuery = _dbContext.Set<Identity>().AsQueryable();

        if (query.FirstName != null)
            identitiesQuery = identitiesQuery.Where(identity => identity.FirstName.Contains(query.FirstName));

        if (query.LastName != null)
            identitiesQuery = identitiesQuery.Where(identity => identity.LastName.Contains(query.LastName));


        var resultQuery = loginsQuery
            .Join(identitiesQuery,
                login => login.Identity.Id,
                identity => identity.Id,
                (login, detail) => new LoginDto
                {
                    Id = login.Id,
                    LoginName = login.LoginName,
                    Email = detail.Email,
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

        return resultQuery.GetListPageQuery(query);
    }
}