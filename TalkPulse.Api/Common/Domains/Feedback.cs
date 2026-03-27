namespace TalkPulse.Api.Common.Domains;

public sealed class Feedback
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Text { get; private set; } = string.Empty;

    public string? AudienceFingerprint { get; private set; }

    public FeedbackType Type { get; private set; }
    public int Rating { get; private set; } // 1-5
    public DateTime SubmittedAt { get; private set; } = DateTime.UtcNow;

    public Guid SessionId { get; private set; }
    public Session Session { get; private set; } = default!;

    public static Feedback Create(int rating, string text, Guid sessionId)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));
        
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Feedback text cannot be empty.", nameof(text));

        return new Feedback
        {
            Rating = rating,
            Text = text,
            SessionId = sessionId,
        };
    }
}

public enum FeedbackType
{
    DuringTalk,
    PostTalk
}