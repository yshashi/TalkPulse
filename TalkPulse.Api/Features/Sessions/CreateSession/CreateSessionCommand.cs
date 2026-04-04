using MediatR;
using TalkPulse.Api.Common.Error;

namespace TalkPulse.Api.Features.Sessions.CreateSession;

public sealed record CreateSessionCommand(Guid SpeakerId, string Title, string Description) : IRequest<Result<CreateSessionResponse>>;

public sealed record CreateSessionResponse(Guid SessionId, string JoinCode);