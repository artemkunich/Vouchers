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

    public Result AddError(Error error)
    {
        _errors.Add(error);
        return this;
    } 

    public Result AddErrors(params Error[] errors)
    {
        _errors.AddRange(errors);
        return this;
    }
    
    public Result AddErrorIf(bool condition, Error error)
    {
        if(condition)
            _errors.Add(error);

        return this;
    }
    
    public Result IfSuccess(Action predicate)
    {
        if (IsSuccess)
            predicate();
        
        return this;
    }
    
    public Result IfSuccess<TResult>(Func<Result<TResult>> predicate)
    {
        if (IsFailure)
            return this;
        
        var result = predicate();
        if (result.IsFailure)
            AddErrors(result.Errors);

        return this;
    }

    public static Result Create() => new();
    public static Result Failure(params Error[] errors) => new(errors);
    
    public static Result<TValue> Create<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(params Error[] errors) => new(errors); 
    
    
    public static Result<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        var result = predicate(value);
        if (result)
            return Failure<T>(error);

        return Create(value);
    }
    
    public static Result<T> Process<T>(T value, Action<T> predicate)
    {
        predicate(value);
        return Create(value);
    }
    
    public Result<TOut> Map<TOut>(Func<TOut> ifSuccessPredicate)
    {
        if(IsSuccess)
            return ifSuccessPredicate();

        return Errors;
    }

}