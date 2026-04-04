using MediatR;
using TalkPulse.Api.Common.Extensions;
using TalkPulse.Api.Features.Sessions.GetSessions;

namespace TalkPulse.Api.Features.Sessions;

public sealed class GetSessionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions", async (IMediator mediator, CancellationToken ct) =>
        {
            var speakerId = Guid.Parse("ac50eb2a-e895-46d4-8bbf-986bd47cc9d3");
            var query = new GetSessionQuery(speakerId);
            var result = await mediator.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("GetSessions")
        .WithDescription("Retrieves a list of sessions")
        .WithTags("Sessions");
    }
}