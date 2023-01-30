namespace Vouchers.Primitives;

public class Error : IEquatable<Error>
{
    public static Error None = new Error(string.Empty, String.Empty);

    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = code;
    }


    public bool Equals(Error? other)
    {
        if (other is null)
            return false;

        if (Code == other.Code)
            return true;

        return false;
    }

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

}