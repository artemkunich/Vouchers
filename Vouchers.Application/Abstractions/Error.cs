using System;
using System.Globalization;
using Vouchers.Application.UseCases;

namespace Vouchers.Application.Abstractions;

public abstract class Error : IEquatable<Error>
{
    public string Code => GetType().Name.Replace("Error", "");
    public string Message;
    
    public bool Equals(object other)
    {
        if (other is null)
            return false;

        if(other is Error error)
            return Code == error.Code;

        return false;
    }
    
    public bool Equals(Error other)
    {
        if (other is null)
            return false;

        return Code == other.Code;
    }

    public static bool operator == (Error a, Error b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator != (Error a, Error b) => !(a == b);

}