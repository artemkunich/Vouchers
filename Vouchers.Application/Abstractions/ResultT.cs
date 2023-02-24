using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public class Result<TValue> : Result
{
    private readonly TValue _value;

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
    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
    
    public override Result<TValue> IfTrueAddError(bool condition, Error error)
    {
        if(condition)
            AddError(error);

        return this;
    }

    public Result<TValue> IfValueIsNullAddError(Error error)
    {
        if (IsFailure)
            return this;
            
        if(Value is null)
            AddError(error);

        return this;
    }
    
    public Result<TValue> IfTrueAddError(Func<TValue,bool> ifSuccessPredicate, Error error)
    {
        if (IsFailure)
            return this;
            
        if(ifSuccessPredicate(Value))
            AddError(error);

        return this;
    }
    
    public async Task<Result<TValue>> IfTrueAddErrorAsync(Func<TValue,Task<bool>> ifSuccessPredicate, Error error)
    {
        if (IsFailure)
            return this;
            
        if(await ifSuccessPredicate(Value))
            AddError(error);

        return this;
    }
    
    public Result<TValue> Process(Action<TValue> ifSuccessPredicate)
    {
        if (IsSuccess)
            ifSuccessPredicate(Value);
        
        return this;
    }
    
    public async Task<Result<TValue>> ProcessAsync(Func<TValue, Task> ifSuccessPredicate)
    {
        if (IsSuccess)
            await ifSuccessPredicate(Value);
        
        return this;
    }

    public Result<TOut> Map<TOut>(Func<TValue, TOut> ifSuccessPredicate) =>
        IsSuccess ? ifSuccessPredicate(Value) : Errors;
    
    public async Task<Result<TOut>> MapAsync<TOut>(Func<TValue, Task<TOut>> ifSuccessPredicate) =>
        IsSuccess ? await ifSuccessPredicate(Value) : Errors;
}