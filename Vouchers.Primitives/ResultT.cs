using System.Collections;
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
    
    public override Result<TValue> MergeResultErrors(Result result)
    {
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }
    
    public Result<TValue> MergeResultErrors(Func<TValue,Result> ifSuccessPredicate)
    {
        if (IsFailure)
            return this;
        
        var result = ifSuccessPredicate(Value);
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }

    public async Task<Result<TValue>> MergeResultErrorsAsync(Func<TValue,Task<Result>> ifSuccessPredicate)
    {
        if (IsFailure)
            return this;
        
        var result = await ifSuccessPredicate(Value);
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }

    public Result<TValue> ForeachWhileSuccess<TItemValue>(Func<TValue,IEnumerable<TItemValue>> enumerable, Func<TItemValue, TValue, Result> predicate)
    {
        if (IsFailure)
            return this;

        foreach (var item in enumerable(Value))
        {
            var result = predicate(item, Value);
            if (result.IsSuccess) continue;
            AddErrors(result.Errors);
            break;
        }
        
        return this;
    }
    
    public async Task<Result<TValue>> ForeachWhileSuccessAsync<TItemValue>(Func<TValue,IEnumerable<TItemValue>> enumerable, Func<TItemValue, TValue, Task<Result>> predicate)
    {
        if (IsFailure)
            return this;

        foreach (var item in enumerable(Value))
        {
            var result = await predicate(item, Value);
            if (result.IsSuccess) continue;
            AddErrors(result.Errors);
            break;
        }
        
        return this;
    }
    
    public Result<TOut> Map<TOut>(Func<TValue, TOut> ifSuccessPredicate) =>
        IsSuccess ? ifSuccessPredicate(Value) : Errors;
    
    public async Task<Result<TOut>> MapAsync<TOut>(Func<TValue, Task<TOut>> ifSuccessPredicate) =>
        IsSuccess ? await ifSuccessPredicate(Value) : Errors;

    
    
    public Result<TOut> ToResult<TOut>(Result<TOut> result) =>
        IsSuccess ? result : Errors;
    
    public Result<TOut> ToResult<TOut>(Func<TValue, Result<TOut>> ifSuccessPredicate) =>
        IsSuccess ? ifSuccessPredicate(Value) : Errors;
    
    public async Task<Result<TOut>> ToResultAsync<TOut>(Func<TValue, Task<Result<TOut>>> ifSuccessPredicate) =>
        IsSuccess ? await ifSuccessPredicate(Value) : Errors;
}