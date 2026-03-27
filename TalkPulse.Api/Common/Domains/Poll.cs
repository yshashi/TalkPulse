using System.ComponentModel.DataAnnotations;

namespace TalkPulse.Api.Common.Domains;
public sealed class Poll
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Question { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Optimistic concurrency control: Track total votes to detect concurrent updates

    [Timestamp]
    public byte[] RowVersion { get; private set; } = [];
    public IReadOnlyCollection<PollOption> Options { get; private set; } = [];


    public Guid SessionId { get; private set; }
    public Session Session { get; private set; } = default!;

    public static Poll Create(string question, IEnumerable<PollOption> options, Guid sessionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(question);

        if (!options.Any())
            throw new ArgumentException("At least one option is required.", nameof(options));

        var poll = new Poll
        {
            Question = question,
            SessionId = sessionId
        };

        poll.Options = [.. options.Select(o => PollOption.Create(o.Text, poll.Id))];
        
        return poll;

    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}