using System;

namespace Utils.Exceptions;

public class ResultException : Exception
{
    public int Code { get; }

    public ResultException() : base()
    {
    }

    public ResultException(string message) : base(message)
    {
    }

    public ResultException(string message, int code) : base(message)
    {
        Code = code;
    }

    public ResultException(string message, int code, Exception innerException) : base(message, innerException)
    {
        Code = code;
    }

    public ResultException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ResultException(Exception innerException) : base(innerException.Message, innerException)
    {
    }
}
