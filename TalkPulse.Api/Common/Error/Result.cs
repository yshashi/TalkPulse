namespace TalkPulse.Api.Common.Error;

public sealed class Result<T>
{
    private readonly T? _value;
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        _value = value;
        IsSuccess = true;
        Error = Error.None;
    }

    private Result(Error error)
    {
        _value = default;
        IsSuccess = false;
        Error = error;
    }

    public T GetValue()
    {
        if (IsFailure)
            throw new InvalidOperationException("Cannot get value from a failed result.");

        return _value!;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
}