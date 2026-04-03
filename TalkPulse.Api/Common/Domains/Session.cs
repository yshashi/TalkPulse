namespace TalkPulse.Api.Common.Domains;

public sealed class Session
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public SessionStatus Status { get; private set; } = SessionStatus.Draft;

    public string JoinCode { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime StartedAt { get; private set; }
    public DateTime EndedAt { get; private set; }

    public Guid SpeakerId { get; private set; }
    public Speaker Speaker { get; private set; } = default!;

    public IReadOnlyCollection<Feedback> Feedbacks { get; private set; } = new List<Feedback>();
    public IReadOnlyCollection<Poll> Polls { get; private set; } = new List<Poll>();

    public static Session Create(string title, string? description, Guid speakerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new Session
        {
            Title = title,
            Description = description,
            SpeakerId = speakerId,
            JoinCode = GenerateJoinCode(),
        };
    }

    public void GoLive()
    {
        if (Status != SessionStatus.Draft)
            throw new InvalidOperationException("Only draft sessions can be started.");

        Status = SessionStatus.Live;
        StartedAt = DateTime.UtcNow;
    }

    public void End()
    {
        if (Status != SessionStatus.Live)
            throw new InvalidOperationException("Only live sessions can be ended.");

        Status = SessionStatus.Ended;
        EndedAt = DateTime.UtcNow;
    }

    private static string GenerateJoinCode()
    {
        return Random.Shared.Next(100000, 999999).ToString(); // Simple 6-digit code for demo purposes
    }
}

public enum SessionStatus
{
    Draft,
    Live,
    Ended
}