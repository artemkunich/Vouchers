using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.API.Services;

public sealed class Mapper<T,TResult> : IMapper<T,TResult>
{
    private readonly AutoMapper.IMapper _mapper;

    public Mapper(AutoMapper.IMapper mapper) =>
        _mapper = mapper;

    public TResult Map(T obj)
    {
        return _mapper.Map<TResult>(obj);
    }
}