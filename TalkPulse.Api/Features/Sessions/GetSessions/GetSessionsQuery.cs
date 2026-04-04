using MediatR;

namespace TalkPulse.Api.Features.Sessions.GetSessions;

public sealed record GetSessionQuery(Guid SpeakerId) : IRequest<List<SessionsSummaryResponse>>;

public sealed record SessionsSummaryResponse(
    Guid Id,
    string Title,
    string? Description,
    string SpeakerName,
    string JoinCode
    );