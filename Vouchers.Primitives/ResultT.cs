using System.Dynamic;

namespace Vouchers.Primitives;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue value, params Error[] errors) : base(errors)
    {
        _value = value;
    }

    protected internal Result(params Error[] errors) : base(errors)
    { }
    
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException();

    public static implicit operator Result<TValue>(TValue value) => Create(value);
    public static implicit operator Result<TValue>(Error[] errors) => Failure<TValue>(errors.ToArray());
    
    public Result<TValue> AddErrorIf(bool condition, Error error)
    {
        if(condition)
            AddError(error);

        return this;
    }

    public Result<TValue> IfSuccess(Action<TValue> predicate)
    {
        if (IsSuccess)
            predicate(Value);
        
        return this;
    }
    
    public Result<TValue> IfSuccess<TResult>(Func<TValue,Result<TResult>> predicate)
    {
        if (IsFailure)
            return this;
        
        var result = predicate(Value);
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }
    
    public Result<TOut> Map<TOut>(Func<TValue, TOut> ifSuccessPredicate)
    {
        if(IsSuccess)
            return ifSuccessPredicate(Value);

        return Errors;
    }
}