using MediatR;
using Microsoft.EntityFrameworkCore;
using TalkPulse.Api.Common.Persistence;

namespace TalkPulse.Api.Features.Sessions.GetSessions;

internal sealed class GetSessionsQueryHandler(AppDbContext db) : IRequestHandler<GetSessionQuery, List<SessionsSummaryResponse>>
{

    public async Task<List<SessionsSummaryResponse>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var sessions = await db.Sessions
            .Where(s => s.SpeakerId == request.SpeakerId)
            .Select(s => new SessionsSummaryResponse(
                s.Id,
                s.Title,
                s.Description,
                s.Speaker.Name,
                s.JoinCode
            )).ToListAsync(cancellationToken);

        return sessions;
    }
}
