using TalkPulse.Api.Common.Error;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.GetValue());

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Error),
            ErrorType.NotFound => Results.NotFound(result.Error),
            ErrorType.Conflict => Results.Conflict(result.Error),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.Problem(result.Error.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}