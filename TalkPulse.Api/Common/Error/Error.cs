namespace TalkPulse.Api.Common.Error;

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict,
    Internal,
    Unauthorized,
    Forbidden
}

public sealed record Error(ErrorType Type, string ErrorMessage)
{
    public static readonly Error None = new(ErrorType.None, string.Empty);
    public static Error Validation(string message = "Validation Error") => new(ErrorType.Validation, message);
    public static Error NotFound(string message = "Resource not found") => new(ErrorType.NotFound, message);
    public static Error Conflict(string message = "Conflict occurred") => new(ErrorType.Conflict, message);
    public static Error Internal(string message = "Internal server error") => new(ErrorType.Internal, message);
    public static Error Unauthorized(string message = "Unauthorized access") => new(ErrorType.Unauthorized, message);
    public static Error Forbidden(string message = "Forbidden access") => new(ErrorType.Forbidden, message);
}
