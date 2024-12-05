using System.Collections.Generic;
using System;
using Utils.Exceptions;

namespace Utils;

public abstract class ResultBase
{
    public bool IsSuccess { get; protected set; }
    public ResultException Exception { get; protected set; }
    public string ErrorMessage { get; protected set; }
    public int ErrorCode { get; protected set; }

    protected ResultBase()
    {
        IsSuccess = true;
    }

    protected ResultBase(ResultException exception)
    {
        IsSuccess = false;
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        ErrorMessage = exception.Message;
        ErrorCode = exception.Code;
    }
}

public sealed class Result<T> : ResultBase
{
    internal Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    internal Result() { }
    internal Result(ResultException exception) : base(exception) { }

    public T? Value { get; }

    public static implicit operator T(Result<T> result) => result.Value;

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is Result<T> otherResult)
        {
            return EqualityComparer<T>.Default.Equals(Value, otherResult.Value);
        }
        if (obj is T genericResult)
        {
            return EqualityComparer<T>.Default.Equals(Value, genericResult);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value == null ? 0 : Value.GetHashCode();
    }

    public static bool operator ==(Result<T> left, Result<T> right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Result<T> left, Result<T> right)
    {
        return !(left == right);
    }

    public static bool operator ==(Result<T> left, T right)
    {
        if (left is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Result<T> left, T right)
    {
        return !(left == right);
    }

    public static bool operator ==(T left, Result<T> right)
    {
        return right == left;
    }

    public static bool operator !=(T left, Result<T> right)
    {
        return !(left == right);
    }
}

public sealed class Result : ResultBase
{
    internal Result() { }
    internal Result(ResultException exception) : base(exception) { }

    public static Result Success() => new Result();
    public static Result<T> Success<T>(T result) => new Result<T>(result);

    public static Result Fail(string errorMessage) => new Result(new ResultException(errorMessage));
    public static Result Fail(string errorMessage, Exception exception) => new Result(new ResultException(errorMessage, exception));
    public static Result Fail(string errorMessage, int errorCode) => new Result(new ResultException(errorMessage, errorCode));
    public static Result Fail(string errorMessage, int errorCode, Exception exception) => new Result(new ResultException(errorMessage, errorCode, exception));
    public static Result Fail(ResultException exception) => new Result(exception);

    public static Result<T> Fail<T>(string errorMessage) => new Result<T>(new ResultException(errorMessage));
    public static Result<T> Fail<T>(string errorMessage, Exception exception) => new Result<T>(new ResultException(errorMessage, exception));
    public static Result<T> Fail<T>(string errorMessage, int errorCode) => new Result<T>(new ResultException(errorMessage, errorCode));
    public static Result<T> Fail<T>(string errorMessage, int errorCode, Exception exception) => new Result<T>(new ResultException(errorMessage, errorCode, exception));
    public static Result<T> Fail<T>(ResultException exception) => new Result<T>(exception);
}
