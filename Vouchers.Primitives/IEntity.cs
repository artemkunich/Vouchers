namespace Vouchers.Primitives;

public interface IEntity<TKey>
{
    TKey Id { get; }
}