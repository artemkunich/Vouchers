using Vouchers.Application.Infrastructure;

namespace Vouchers.MVC.Services
{
    public class Mapper<T,TResult> : IMapper<T,TResult>
    {
        private readonly AutoMapper.IMapper mapper;

        public Mapper(AutoMapper.IMapper mapper) =>
            this.mapper = mapper;

        public TResult Map(T obj)
        {
            return mapper.Map<TResult>(obj);
        }
    }
}
