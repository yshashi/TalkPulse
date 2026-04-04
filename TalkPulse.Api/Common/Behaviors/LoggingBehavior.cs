using MediatR;

namespace TalkPulse.Api.Common.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("-> Handling {RequestName} with content: {@Request}", requestName, request);

        var response = await next(ct);

        logger.LogInformation("<- Handled {RequestName} with response: {@Response}", requestName, response);

        return response;
    }
}