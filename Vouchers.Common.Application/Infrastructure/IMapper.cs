namespace Vouchers.Common.Application.Infrastructure;

public interface IMapper<in T, out TResult>
{
    TResult Map(T entity);
}