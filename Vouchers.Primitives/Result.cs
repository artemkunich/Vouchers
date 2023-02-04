using System.Runtime.InteropServices.JavaScript;

namespace Vouchers.Primitives;

public class Result
{
    public bool IsSuccess => !IsFailure;
    public bool IsFailure => _errors.Any();

    private readonly List<Error> _errors;
    public Error[] Errors => _errors.ToArray();
    
    protected Result(params Error[] errors)
    {
        _errors = new List<Error>(errors);
    }
    
    protected Result AddError(Error error)
    {
        _errors.Add(error);
        return this;
    }

    protected Result AddErrors(params Error[] errors)
    {
        _errors.AddRange(errors);
        return this;
    }
    
    public Result<TOut> SetValue<TOut>(TOut value)
    {
        if(IsSuccess)
            return value;

        return Errors;
    }
    
    public virtual Result IfTrueAddError(bool condition, Error error)
    {
        if(condition)
            _errors.Add(error);

        return this;
    }

    public Result Process(Action predicate)
    {
        if (IsSuccess)
            predicate();
        
        return this;
    }
    
    public virtual Result MergeResultErrors(Result result)
    {
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }

    public Result MergeResultErrors(Func<Result> ifSuccessPredicate)
    {
        if (IsFailure)
            return this;
        
        var result = ifSuccessPredicate();
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }
    
    public virtual Result ForeachWhileSuccess<TValue,TResult>(IEnumerable<TValue> enumerable, Func<TValue, Result<TResult>> predicate)
    {
        if (IsFailure)
            return this;

        foreach (var item in enumerable)
        {
            var result = predicate(item);
            if (result.IsFailure)
            {
                AddErrors(result.Errors);
                break;
            }
        }
        
        return this;
    }
    
    public virtual async Task<Result> ForeachWhileSuccessAsync<TValue,TResult>(IEnumerable<TValue> enumerable, Func<TValue, Task<Result<TResult>>> predicate)
    {
        if (IsFailure)
            return this;

        foreach (var item in enumerable)
        {
            var result = await predicate(item);
            if (result.IsFailure)
            {
                AddErrors(result.Errors);
                break;
            }
        }
        
        return this;
    }
    
    public static Result Create() => new();
    public static Result Failure(params Error[] errors) => new(errors);
    
    public static Result<TValue> Create<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(params Error[] errors) => new(errors);
    
}