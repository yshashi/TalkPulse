namespace TalkPulse.Api.Common.Domains;

public sealed class PollOption
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Text { get; private set; } = string.Empty;
    public int VoteCount { get; private set; }

    public Guid PollId { get; private set; }
    public Poll Poll { get; private set; } = default!;

    public static PollOption Create(string text, Guid pollId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        return new PollOption
        {
            Text = text,
            PollId = pollId
        };  
    }

    public void IncrementVote() => VoteCount++;
}