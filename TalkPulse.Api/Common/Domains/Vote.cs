namespace TalkPulse.Api.Common.Domains;

// Will verify it in next session (Stream No 3)
public sealed class Vote
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PollOptionId { get; private set; }
    public PollOption PollOption { get; private set; } = default!;

    public Guid PollId { get; private set; }

    public string? AudienceFingerprint { get; private set; }
    public DateTime VotedAt { get; private set; } = DateTime.UtcNow;

    public Guid SessionId { get; private set; }
    public Session Session { get; private set; } = default!;

    public static Vote Create(Guid pollOptionId, Guid pollId, Guid sessionId, string? audienceFingerprint = null)
    {
        return new Vote
        {
            PollOptionId = pollOptionId,
            PollId = pollId,
            SessionId = sessionId,
            AudienceFingerprint = audienceFingerprint
        };
    }
}