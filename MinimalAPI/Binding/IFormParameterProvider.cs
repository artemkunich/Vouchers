namespace Vouchers.MinimalAPI.Binding;

public interface IFormParameterProvider<T> where T: class
{
    public T GetParameter();
}