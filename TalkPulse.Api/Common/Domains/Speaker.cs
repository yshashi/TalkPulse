namespace TalkPulse.Api.Common.Domains;

public sealed class Speaker
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    // Speakers will have sessions
    public IReadOnlyCollection<Session> Sessions { get; private set; } = new List<Session>();


    public static Speaker Create(string name, string email, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new Speaker
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash
        };
    }
}