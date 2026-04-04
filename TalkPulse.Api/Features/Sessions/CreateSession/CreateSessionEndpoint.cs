using MediatR;
using TalkPulse.Api.Common.Extensions;

namespace TalkPulse.Api.Features.Sessions.CreateSession;

internal sealed class CreateSessionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/sessions", async (CreateSessionRequest request, ISender sender, CancellationToken ct) =>
        {
            var speakerId = Guid.Parse("ac50eb2a-e895-46d4-8bbf-986bd47cc9d3");
            var command = new CreateSessionCommand(speakerId, request.Title, request.Description);
            var result = await sender.Send(command, ct);

            return result.ToHttpResult();
        })
        .WithName("CreateSession")
        .WithDescription("Creates a new session")
        .WithTags("Sessions");
    }
}