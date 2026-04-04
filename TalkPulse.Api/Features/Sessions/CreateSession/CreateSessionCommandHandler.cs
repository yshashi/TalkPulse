using MediatR;
using Microsoft.EntityFrameworkCore;
using TalkPulse.Api.Common.Domains;
using TalkPulse.Api.Common.Error;
using TalkPulse.Api.Common.Persistence;

namespace TalkPulse.Api.Features.Sessions.CreateSession;

public sealed record CreateSessionCommandHandler(AppDbContext db) : IRequestHandler<CreateSessionCommand, Result<CreateSessionResponse>>
{
    public async Task<Result<CreateSessionResponse>> Handle(CreateSessionCommand request, CancellationToken ct)
    {
        var speakerExists = await db.Speakers.AnyAsync(s => s.Id == request.SpeakerId, ct);
        if (!speakerExists)
            return Result<CreateSessionResponse>.Failure(Error.NotFound("Speaker not found"));

        var session = Session.Create(request.Title, request.Description, request.SpeakerId);

        db.Sessions.Add(session);
        await db.SaveChangesAsync(ct);

        return Result<CreateSessionResponse>.Success(new CreateSessionResponse(session.Id, session.JoinCode));
    }
}