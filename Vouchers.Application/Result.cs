using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vouchers.Application;

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

    public Result Process(Action ifSuccessPredicate)
    {
        if (IsSuccess)
            ifSuccessPredicate();
        
        return this;
    }

    public static Result Create() => new();
    public static Result Failure(params Error[] errors) => new(errors);
    
    public static Result<TValue> Create<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(params Error[] errors) => new(errors);
    
    public static implicit operator Result(Error[] errors) => Failure(errors.ToArray());
    public static implicit operator Result(Error error) => Failure(error);
    
}